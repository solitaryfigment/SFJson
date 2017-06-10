using System;
using System.Collections.Generic;
using System.Text;

namespace SFJson
{
	public class Parser
    {
        private static Stack<JsonToken> _stack = new Stack<JsonToken>();
        private static JsonToken _currentToken = null;
        private static bool _isQuote = false;
        private static string _tokenName = string.Empty;
        private static StringBuilder _tokenText = new StringBuilder();

	    private static string _jsonString;
	    private static int _index;

	    private static bool HandleIfQuoted()
	    {
		    var currentChar = _jsonString[_index];
			switch(currentChar)
			{
				// TODO: Handle Escape Characters & Unicode		
				case '"':
					_isQuote = !_isQuote;
					_currentToken.IsQuoted |= _isQuote;
					return true;
			}
		    if(_isQuote)
		    {
			    _tokenText.Append(currentChar);
			    return true;
		    }
		    return false;
	    }

	    private static void PushToken<T>() where T : JsonToken, new()
	    {
		    T token = new T();
		    if(_stack.Count > 0)
		    {
			    Console.WriteLine("Appending To: " + _currentToken.Name + " : " + _currentToken.JsonType + " : " + (_currentToken.ChildElements.Count + 1));
			    token.Name = _tokenName;
			    _currentToken.ChildElements.Add(token);
		    }
		    _currentToken = token;
		    _stack.Push(token);
	    }

	    private static void PopToken<T>()
	    {
		    Console.WriteLine("Is: " + (_currentToken is T));
		    // TODO: Validate TokenType
		    _stack.Pop();
		    ParseElement();
		    if(_stack.Count > 0)
		    {
			    _currentToken = _stack.Peek();
		    }
	    }
	    
        public static JsonToken Tokenize(string jsonString)
        {
	        _tokenText.Length = 0;
	        _jsonString = jsonString;
            for(_index = 0; _index < _jsonString.Length; _index++)
            {
	            if(HandleIfQuoted())
	            {
		            continue;
	            }
	            
                var currentChar = _jsonString[_index];
                switch(currentChar)
                {
	                case '{':
		                PushToken<JsonObject>();
						ResetTokenText();
		                break;
	                case '}':
		                PopToken<JsonObject>();
		                ResetTokenText();
		                break;
	                case '[':
		                PushToken<JsonArray>();
						ResetTokenText();
		                break;
	                case ']':
		                PopToken<JsonArray>();
		                ResetTokenText();
		                break;
                    case ':':
						ResetTokenText();
	                    break;
					case ',':
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

	    private static void ResetTokenText()
	    {
		    _tokenName = _tokenText.ToString();
		    _currentToken.IsQuoted = false;
		    _isQuote = false;
		    _tokenText.Length = 0;
	    }

	    private static void ParseElement()
	    {
		    double val;
		    if(_tokenText.Length <= 0)
		    {
			    Console.Write("Did not Append");
			    return;
		    }
		    Console.WriteLine("Appending To: " + _currentToken.Name + " : " + _currentToken.JsonType + " : " + (_currentToken.ChildElements.Count + 1));

		    if(_currentToken.IsQuoted)
		    {
			    _currentToken.ChildElements.Add(new JsonValue(_tokenName, _tokenText.ToString(), JsonType.Value));
			    return;
		    }
		    
		    string tokenText = _tokenText.ToString().ToLower();

		    if(tokenText == "false" || tokenText == "true")
		    {
			    _currentToken.ChildElements.Add(new JsonValue(_tokenName, tokenText == "true", JsonType.Value));
		    }
		    else if(tokenText == "null")
		    {
			    _currentToken.ChildElements.Add(new JsonValue(_tokenName, null, JsonType.Value));
		    }
	    	else if(double.TryParse(tokenText, out val))
		    {
			    _currentToken.ChildElements.Add(new JsonValue(_tokenName, val, JsonType.Value));
		    }
		    else
		    {
			    _currentToken.ChildElements.Add(new JsonValue(_tokenName, _tokenText.ToString(), JsonType.Value));
		    }
		}
    }
}