namespace Shop.Model
{
    public class AgeConfig
    {
        private string _key;
        private int? _max;
        private int? _min;
        public AgeConfig(string key, int? min, int? max) 
        {
            this._key = key;
            this._min = min;
            this._max = max;
        }
        public string Key
        {
            get => _key;
            set => _key = value;
        }
        public int? Max
        {
            get => _max;
            set => _max = value;
        }
        public int? Min
        {
            get => _min;
            set => _min = value;
        }
    }
}
