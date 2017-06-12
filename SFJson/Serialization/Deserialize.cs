namespace SFJson
{
    public class Deserializer
    {
        private string _stringToDeserialize;

        public JsonToken LastTokenization { get; private set; }

        public T Deserialize<T>(string stringToSerialize)
        {
            _stringToDeserialize = stringToSerialize;
            LastTokenization = new Tokenizer().Tokenize(_stringToDeserialize);
            var obj = LastTokenization.GetValue<T>();
            return obj;
        }
    }
}
