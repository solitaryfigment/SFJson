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
        private StringBuilder _tokenText;
        private JsonToken _currentToken;
        private JsonDictionary _dictionary;
        private JsonToken _potentialKey;
        private JsonToken _temp;
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
                _tokenText = new StringBuilder();
                Reset();
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

        private void Reset()
        {
            _tokenName = string.Empty;
            _isTokenQuoted = false;
            _isQuote = false;
            _currentToken = null;
            _dictionary = null;
            _potentialKey = null;
            _temp = null;
            _tokenText.Length = 0;
            _index = 0;
            Clear();
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

        private void SetAsDictionaryToken(JsonToken token)
        {
            try
            {
                // TODO: Ignore whitespace
                var nextChar = _jsonString[_index + 1];
                var previousChar = _jsonString[_index - 1];
                var addThingy = (nextChar == Constants.COLON && _potentialKey == null) || (previousChar == Constants.COLON && _potentialKey != null);
                if(!addThingy)
                {
                    _dictionary = null;
                    return;
                }
            }
            catch
            {
                return;
            }
            if(_potentialKey == null)
            {
                _potentialKey = token;
            }
            else if(_dictionary != null)
            {
                _dictionary.Entries.Add(_potentialKey, token);
                _potentialKey = null;
            }
            else
            {
                _potentialKey = null;
            }
        }

        private void HandleNextCharacter()
        {
            switch(_currentChar)
            {
                case Constants.OPEN_CURLY:
                    PushToken<JsonObject>();
                    SetAsDictionaryToken(_currentToken);
                    break;
                case Constants.CLOSE_CURLY:
                    _temp = _currentToken;
                    PopToken<JsonObject>();
                    SetAsDictionaryToken(_temp);
                    break;
                case Constants.OPEN_BRACKET:
                    PushToken<JsonCollection>();
                    SetAsDictionaryToken(_currentToken);
                    break;
                case Constants.CLOSE_BRACKET:
                    _temp = _currentToken;
                    PopToken<JsonCollection>();
                    SetAsDictionaryToken(_temp);
                    break;
                case Constants.COLON:
                    if(_potentialKey is JsonObject && _dictionary == null)
                    {
                        StartDictionary();
                    }
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
            T token = CreateToken<T>();
            AddAsChildOfCurrentToken(token);
            _currentToken = token;
            Push(token);
            ResetTokenText();
        }
        
        private void AddAsChildOfCurrentToken(JsonToken token)
        {
            if(_currentToken == null)
            {
                token.Name = (string.IsNullOrEmpty(token.Name)) ? "ROOT" : token.Name;
                return;
            }
            
            if(Count > 0 &&  _currentToken.JsonTokenType != JsonTokenType.Dictionary)
            {
                
                _currentToken.Children.Add(token);
            }
        }

        private T CreateToken<T>() where T : JsonToken, new()
        {
            T token = new T();
            token.SettingsManager = _settingsManager;
            token.Name = _tokenName;
            return token;
        }

        private void PopToken<T>()
        {
            if(_currentToken is T || _currentToken is JsonDictionary)
            {
                Pop();
                AddAndParseElement();
                if(Count > 0)
                {
                    _currentToken = Peek();
                }
                _dictionary = _currentToken as JsonDictionary;
                ResetTokenText();
            }
            else
            {
                var expectedCharacter = (_currentToken.JsonTokenType == JsonTokenType.Object) ? "\'}\'" : "\']\'";
                throw new TokenizationException(string.Format("Malformed input: Expected {0} but was \'{1}\' at position {2}.", expectedCharacter, _currentChar.ToString().ToLiteral(), _index));
            }
        }

        private void StartDictionary()
        {
            JsonToken parentToken = null;
            if(_currentToken != null)
            {
                parentToken = (Count > 0) ? Pop() : null;
                _currentToken = (Count > 0) ? Peek() : null;
            }
            PushDictionary(parentToken);
        }

        private void PushDictionary(JsonToken parentToken)
        {
            if(parentToken != null)
            {
                _currentToken.Children.Remove(parentToken);
            }
            _tokenName = (parentToken != null) ? parentToken.Name : "";
            PushToken<JsonDictionary>();
            _dictionary = _currentToken as JsonDictionary;
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
