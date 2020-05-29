using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JSONSAMPLE
{
    using KeyValueMap = Dictionary<string, KeyValue>;

    struct KeyValue
    {
        string _key;
        string _Data;


        public string Key
        {
            get => _key;
            set
            {
                _key = value;
            }
        }

        public string Data
        {
            get => _Data;
            set
            {
                _Data = value;
            }
        }

        public KeyValue(string Key, string Value)
        {
            _key = Key;
            _Data = Value;
        }


        public void Parse(JObject j)
        {
            _key = j["key"].ToString();
            _Data = j["Value"].ToString();
        }

        public JObject Store()
        {
            JObject j = new JObject();

            j.Add("key", _key);
            j.Add("Value", _Data);

            return j;
        }
    }

    class SectionKeyJSON
    {
        Dictionary<string, KeyValueMap> _data = new Dictionary<string, KeyValueMap>();

        string _path;

        public void WriteData(string Section, string Key, string value)
        {

            if (_data.ContainsKey(Section))
            {

                if (_data[Section].ContainsKey(Key))
                {
                    _data[Section][Key] = new KeyValue(Key, value);
                }
                else
                {
                    _data[Section].Add(Key, new KeyValue(Key, value));
                }
            }
            else
            {
                _data[Section] = new KeyValueMap();
                _data[Section].Add(Key, new KeyValue(Key, value));
            }
        }

        void WriteData(string Section, JObject Value)
        {
            KeyValue temp = new KeyValue();
            temp.Parse(Value);

            WriteData(Section, temp.Key, temp.Data);
        }

        public string ReadData(string Section, string Key)
        {
            if (_data.ContainsKey(Section))
            {

                if (_data[Section].ContainsKey(Key))
                {
                    return _data[Section][Key].Data;
                }
            }

            return null;
        }

        public JObject Store()
        {

            JObject j = new JObject();

            foreach (KeyValuePair<string, KeyValueMap> Sections in _data)
            {
                JArray a = new JArray();

                foreach (KeyValuePair<string, KeyValue> Keys in Sections.Value)
                {
                    a.Add(Keys.Value.Store());
                }

                j.Add(Sections.Key, a);
            }

            return j;
        }


        public void Read(string name)
        {
            _path = name;
            var obj = Read();

            foreach (var data in obj)
            {
                JArray a = data.Value as JArray;

                for (int i = 0; i < a.Count; i++)
                {

                    WriteData(data.Key, a[i] as JObject);
                }
            }
        }

        JObject Read()
        {
            JObject obj;

            using (StreamReader file = File.OpenText(_path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                obj = (JObject)JToken.ReadFrom(reader);
            }

            return obj;
        }

        public void Write()
        {
            Write(Store(), _path);
        }

        public void Write(string path)
        {
            Write(Store(), path);
        }

        void Write(JObject j, string name)
        {
            using (StreamWriter file = File.CreateText(name))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                writer.Indentation = 4;
                writer.Formatting = Formatting.Indented;
                j.WriteTo(writer);
            }
        }
    }
}
