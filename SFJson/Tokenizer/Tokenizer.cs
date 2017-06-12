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
	    private int _index;

	    public JsonToken Tokenize(string jsonString)
	    {
		    _tokenText.Length = 0;
		    _jsonString = jsonString;
		    return Tokenize();
	    }
	    
        private JsonToken Tokenize()
        {
            for(_index = 0; _index < _jsonString.Length; _index++)
            {
	            if(HandleIfQuoted())
	            {
		            continue;
	            }
	            
                var currentChar = _jsonString[_index];
                switch(currentChar)
                {
	                case TokenizerConstants.OPEN_CURLY:
		                PushToken<JsonObject>();
						ResetTokenText();
		                break;
	                case TokenizerConstants.CLOSE_CURLY:
		                PopToken<JsonObject>();
		                ResetTokenText();
		                break;
	                case TokenizerConstants.OPEN_BRACKET:
		                PushToken<JsonArray>();
						ResetTokenText();
		                break;
	                case TokenizerConstants.CLOSE_BRACKET:
		                PopToken<JsonArray>();
		                ResetTokenText();
		                break;
                    case TokenizerConstants.COLON:
						ResetTokenText();
	                    break;
					case TokenizerConstants.COMMA:
						ParseElement();
						ResetTokenText();
						break;
                    default:
                        _tokenText.Append(currentChar);
                        break;
                }
	            
	            Console.WriteLine(_tokenText.ToString());
            }

            return _currentToken;
        }

	    private bool HandleIfQuoted()
	    {
			var currentChar = _jsonString[_index];
			if(currentChar == TokenizerConstants.QUOTE)
			{
				_isQuote = !_isQuote;
				_currentToken.IsQuoted |= _isQuote;
				return true;
			}
			if(_isQuote)
			{
				// TODO: Handle Escape Characters & Unicode	
				_tokenText.Append(currentChar);
				return true;
			}
			return false;
	    }

	    private void PushToken<T>() where T : JsonToken, new()
	    {
		    T token = new T();
		    if(Count > 0)
		    {
			    token.Name = _tokenName;
			    Console.WriteLine("Appending :" + _tokenName + ":To: " + _currentToken.Name + " : " + _currentToken.JsonType + " : " + (_currentToken.Children.Count + 1));
			    _currentToken.Children.Add(token);
		    }
		    _currentToken = token;
		    Push(token);
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
	    }

	    private void ResetTokenText()
	    {
		    _tokenName = _tokenText.ToString();
		    _currentToken.IsQuoted = false;
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

		    if(_currentToken.IsQuoted)
		    {
			    _currentToken.Children.Add(new JsonValue(_tokenName, _tokenText.ToString()));
			    return;
		    }
		    
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