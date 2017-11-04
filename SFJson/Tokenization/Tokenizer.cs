using System;
using System.Collections.Generic;
using System.Text;
using SFJson.Conversion.Settings;
using SFJson.Exceptions;
using SFJson.Tokenization.Tokens;
using SFJson.Utils;

namespace SFJson.Tokenization
{
    internal class Tokenizer : Stack<JsonToken>
    {
        private readonly StringBuilder _tokenText = new StringBuilder();
        
        private JsonToken _currentToken;
        private bool _isQuote;
        private bool _isTokenQuoted;
        private string _tokenName = string.Empty;
        private string _jsonString;
        private char _currentChar;
        private int _index;
        private SettingsManager _settingsManager;

        internal JsonToken Tokenize(string jsonString, SettingsManager settingsManager)
        {
            try
            {
                _tokenText.Length = 0;
                _jsonString = jsonString;
                _settingsManager = settingsManager;
                return Tokenize();
            }
            catch(TokenizationException)
            {
                throw;
            }
            catch(Exception e)
            {
                throw new TokenizationException(string.Format("Tokenization Error occured on character {0} at position {1}", _currentChar, _index), e);
            }
        }

        private JsonToken Tokenize()
        {
            for(_index = 0; _index < _jsonString.Length; _index++)
            {
                _currentChar = _jsonString[_index];
                if(HandleIfQuoted())
                {
                    continue;
                }

                HandleNextCharacter();
            }

            if(_currentToken == null)
            {
                _currentToken = ParseElement();
            }
            return _currentToken;
        }

        private void HandleNextCharacter()
        {
            switch(_currentChar)
            {
                case Constants.OPEN_CURLY:
                    PushToken<JsonObject>();
                    break;
                case Constants.CLOSE_CURLY:
                    PopToken<JsonObject>();
                    break;
                case Constants.OPEN_BRACKET:
                    PushToken<JsonCollection>();
                    break;
                case Constants.CLOSE_BRACKET:
                    PopToken<JsonCollection>();
                    break;
                case Constants.COLON:
                    ResetTokenText();
                    break;
                case Constants.COMMA:
                    AddAndParseElement();
                    ResetTokenText();
                    break;
                default:
                    _tokenText.Append(_currentChar);
                    break;
            }
        }

        private bool HandleIfQuoted()
        {
            if(_currentChar == Constants.QUOTE)
            {
                _isQuote = !_isQuote;
                _isTokenQuoted = true;
                return true;
            }

            if(_currentChar == '\\')
            {
                _index++;
                var nextChar = _jsonString[_index];
                switch(nextChar)
                {
                    case '\"':
                        _tokenText.Append('\"');
                        if(!_isQuote)
                        {
                            _isQuote = true;
                        }
                        break;
                    case 't':
                        _tokenText.Append('\t');
                        break;
                    case 'r':
                        _tokenText.Append('\r');
                        break;
                    case 'n':
                        _tokenText.Append('\n');
                        break;
                    case 'b':
                        _tokenText.Append('\b');
                        break;
                    case 'f':
                        _tokenText.Append('\f');
                        break;
                    case 'u':
                    {
                        string unicodeString = _jsonString.Substring(_index + 1, 4);
                        _tokenText.Append((char) int.Parse(unicodeString, System.Globalization.NumberStyles.AllowHexSpecifier));
                        _index += 4;
                        break;
                    }
                    default:
                        _tokenText.Append(_currentChar);
                        break;
                }

                return true;
            }

            if(_isQuote)
            {
                _tokenText.Append(_currentChar);
                return true;
            }

            return false;
        }

        private void PushToken<T>() where T : JsonToken, new()
        {
            T token = new T();
            token.SettingsManager = _settingsManager;
            if(Count > 0)
            {
                _currentToken.Children.Add(token);
            }
            token.Name = _tokenName;
            _currentToken = token;
            Push(token);
            ResetTokenText();
        }

        private void PopToken<T>()
        {
            if(_currentToken is T)
            {
                Pop();
                AddAndParseElement();
                if(Count > 0)
                {
                    _currentToken = Peek();
                }
                ResetTokenText();
            }
            else
            {
                var expectedCharacter = (_currentToken.JsonTokenType == JsonTokenType.Object) ? "\'}\'" : "\']\'";
                throw new TokenizationException(string.Format("Malformed input: Expected {0} but was \'{1}\' at position {2}.", expectedCharacter, _currentChar.ToString().ToLiteral(), _index));
            }
        }

        private void ResetTokenText()
        {
            _tokenName = _tokenText.ToString();
            _isQuote = false;
            _isTokenQuoted = false;
            _tokenText.Length = 0;
        }

        private JsonToken ParseElement()
        {
            JsonToken token;
            string tokenText = _tokenText.ToString().ToLower();
            if(tokenText == Constants.FALSE || tokenText == Constants.TRUE)
            {
                token = new JsonValue(_tokenName, tokenText == Constants.TRUE);
            }
            else if(tokenText == Constants.NULL)
            {
                token = new JsonValue(_tokenName, null);
            }
            else
            {
                token = new JsonValue(_tokenName, _tokenText.ToString());
            }
            token.SettingsManager = _settingsManager;
            return token;
        }

        private void AddAndParseElement()
        {
            if(_isTokenQuoted || _tokenText.Length > 0)
            {
                _currentToken.Children.Add(ParseElement());
            }
        }
    }
}
