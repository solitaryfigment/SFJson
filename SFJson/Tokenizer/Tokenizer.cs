using System;
using System.Collections.Generic;
using System.Text;

namespace SFJson
{
	public class Tokenizer : Stack<JsonToken>
    {
        private JsonToken _currentToken = null;
        private bool _isQuote = false;
        private string _tokenName = string.Empty;
        private StringBuilder _tokenText = new StringBuilder();
	    private string _jsonString;
	    private char _currentChar;

	    public JsonToken Tokenize(string jsonString)
	    {
		    _tokenText.Length = 0;
		    _jsonString = jsonString;
		    return Tokenize();
	    }
	    
        private JsonToken Tokenize()
        {
            for(int i = 0; i < _jsonString.Length; i++)
            {
	            _currentChar = _jsonString[i];
	            if(HandleIfQuoted())
	            {
		            continue;
	            }
	            HandleNextCharacter();
	            Console.WriteLine(_tokenText.ToString());
            }

            return _currentToken;
        }

	    private void HandleNextCharacter()
	    {
		    switch(_currentChar)
		    {
			    case TokenizerConstants.OPEN_CURLY:
				    PushToken<JsonObject>();
				    break;
			    case TokenizerConstants.CLOSE_CURLY:
				    PopToken<JsonObject>();
				    break;
			    case TokenizerConstants.OPEN_BRACKET:
				    PushToken<JsonArray>();
				    break;
			    case TokenizerConstants.CLOSE_BRACKET:
				    PopToken<JsonArray>();
				    break;
			    case TokenizerConstants.COLON:
				    ResetTokenText();
				    break;
			    case TokenizerConstants.COMMA:
				    ParseElement();
				    ResetTokenText();
				    break;
			    default:
				    _tokenText.Append(_currentChar);
				    break;
		    }
	    }

	    private bool HandleIfQuoted()
	    {
			if(_currentChar == TokenizerConstants.QUOTE)
			{
				_isQuote = !_isQuote;
				return true;
			}
			if(_isQuote)
			{
				// TODO: Handle Escape Characters & Unicode	
				_tokenText.Append(_currentChar);
				return true;
			}
			return false;
	    }

	    private void PushToken<T>() where T : JsonToken, new()
	    {
		    T token = new T();
		    if(Count > 0)
		    {
			    Console.WriteLine("Appending :" + _tokenName + ":To: " + _currentToken.Name + " : " + _currentToken.JsonType + " : " + (_currentToken.Children.Count + 1));
			    _currentToken.Children.Add(token);
		    }
		    token.Name = _tokenName;
		    _currentToken = token;
		    Push(token);
		    ResetTokenText();
	    }

	    private void PopToken<T>()
	    {
		    Console.WriteLine("Is: " + (_currentToken is T));
		    // TODO: Validate TokenType
		    Pop();
		    ParseElement();
		    if(Count > 0)
		    {
			    _currentToken = Peek();
		    }
		    ResetTokenText();
	    }

	    private void ResetTokenText()
	    {
		    _tokenName = _tokenText.ToString();
		    _isQuote = false;
		    _tokenText.Length = 0;
	    }

	    private void ParseElement()
	    {
		    double val;
		    if(_tokenText.Length <= 0)
		    {
			    Console.Write("Did not Append");
			    return;
		    }
		    Console.WriteLine("Appending To: " + _currentToken.Name + " : " + _currentToken.JsonType + " : " + (_currentToken.Children.Count + 1));
		    
		    string tokenText = _tokenText.ToString().ToLower();

		    if(tokenText == TokenizerConstants.FALSE || tokenText == TokenizerConstants.TRUE)
		    {
			    _currentToken.Children.Add(new JsonValue(_tokenName, tokenText == TokenizerConstants.TRUE));
		    }
		    else if(tokenText == TokenizerConstants.NULL)
		    {
			    _currentToken.Children.Add(new JsonValue(_tokenName, null));
		    }
	    	else if(double.TryParse(tokenText, out val))
		    {
			    _currentToken.Children.Add(new JsonValue(_tokenName, val));
		    }
		    else
		    {
			    _currentToken.Children.Add(new JsonValue(_tokenName, _tokenText.ToString()));
		    }
		}
    }
}
