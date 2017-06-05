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

        public static JsonToken Tokenize(string jsonString)
        {
	        _tokenText.Length = 0;
            for(int i = 0; i < jsonString.Length; i++)
            {
                var currentChar = jsonString[i];
                switch(currentChar)
                {
                    case '{':
                        if(_isQuote)
                        {
                            _tokenText.Append(currentChar);
                        }
                        else
                        {
                            var token = new JsonObject();
	                        if(_stack.Count > 0)
	                        {
		                        token.Name = _tokenName;
		                        _currentToken.ChildElements.Add(token);
	                        }
                            _currentToken = token;
                            _stack.Push(token);
                        }
                        break;
                    case '}':
                        if(_isQuote)
                        {
                            _tokenText.Append(currentChar);
                        }
                        _stack.Pop();
	                    ParseElement();
                        if(_stack.Count > 0)
                        {
                            _currentToken = _stack.Peek();
                        }
	                    ResetTokenText();
                        break;
                    case '"':
                        _isQuote = !_isQuote;
                        _currentToken.IsQuoted |= _isQuote;
                        break;
                    case ':':
                        if(_isQuote)
                        {
                            _tokenText.Append(currentChar);
                            break;
                        }
	                    ResetTokenText();
                        break;
					case ',':
						if(_isQuote)
						{
							_tokenText.Append(currentChar);
							break;
						}
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
			    return;
		    }
		    
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