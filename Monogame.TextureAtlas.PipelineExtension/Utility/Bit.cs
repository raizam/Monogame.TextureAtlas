namespace TextureAtlas.PipelineExtension.Utility
{
    //Bit struct. In reference, DFS-order is stored in a 0-1 sequence, 
    //this struct has been created to follow terminology.
    struct Bit
    {
        private bool bit;

        private Bit(int n)
        {
            this.bit = (n == 1) ? true : false;
        }

        public static implicit operator Bit(int n)
        {
            return new Bit(n);
        }

        public override bool Equals(object obj)
        {
            if (obj is Bit)
                return this == ((Bit)obj);
            else if (obj is int)
                return this == (int)obj;

            return false;
        }

        public override int GetHashCode()
        {
            return bit.GetHashCode();
        }

        public static bool operator ==(Bit b, int n)
        {
            if ((b.bit == true && n == 1) || (b.bit == false && n == 0))
                return true;
            else return false;
        }

        public static bool operator !=(Bit b, int n)
        {
            if (!((b.bit == true && n == 1) || (b.bit == false && n == 0)))
                return true;
            else return false;
        }

        public static bool operator ==(Bit b1, Bit b2)
        {
            if ((b1.bit == true && b2.bit == true) || (b1.bit == false && b2.bit == false))
                return true;
            else return false;
        }

        public static bool operator !=(Bit b1, Bit b2)
        {
            if (!((b1.bit == true && b2.bit == true) || (b1.bit == false && b2.bit == false)))
                return true;
            else return false;
        }
    }
}
