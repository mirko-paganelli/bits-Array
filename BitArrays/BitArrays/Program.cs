namespace BitArrays
{
    internal class Program
    {
        class BitArray
        {
            private const int BITS_PER_VALUE = 32;  // ogni (unit) ha 32 bit
            private uint[] bits;
            private long n_bits;

            public BitArray(long n_bits,bool initial_value)
            {
                /*this.n_bits = n_bits;

                int n_units = (int) (n_bits / BITS_PER_VALUE);
                if (n_bits % BITS_PER_VALUE != 0)
                    ++n_units;  // serve un (uint) in più per i bits residui
                bits = new uint[n_units];
                */
                long n_uints = n_bits / BITS_PER_VALUE;
                if (n_bits % BITS_PER_VALUE != 0)
                    ++n_uints;  // serve un (uint) in più per i bits residui

                if (n_uints > long.MaxValue - 60)  // il valore massimo è stato trovato per tentativi
                    throw new OverflowException("Too many bits");

                this.bits = new uint[n_uints];
                this.n_bits = n_bits;
                SetAllBits(initial_value);
            }
            public void SetAllBits(bool value)  // imposta tutti i bit a true o false
            {
                uint _value = 0;
                if (value)
                    _value = ~_value;

                for (int i = 0; i < bits.Length; ++i)
                    bits[i] = _value;
            }
            public bool GetBit(long bit_index) 
            {
                int index = (int)bit_index / BITS_PER_VALUE;
                int position = (int)bit_index % BITS_PER_VALUE;
                int mask = 1 << position;
                int number = (int)bits[index] & mask;
                return (number != 0);
            }
            public void SetBit(long bit_index, bool value) 
            {  
                if (value)
                {
                    int index = (int)bit_index / BITS_PER_VALUE;
                    int position = (int)bit_index % BITS_PER_VALUE;
                    int mask = 1 << position;
                    bits[index] = (uint)(bits[index] | mask);
                }
                else
                {
                    int index = (int)bit_index / BITS_PER_VALUE;
                    int position = (int)bit_index % BITS_PER_VALUE;
                    int mask = ~(1 << position);
                    bits[index] = (uint)(bits[index] & mask);
                }   
            }

            public bool this[int bit_index] { get { return GetBit(bit_index); } set { SetBit(bit_index, value); } }
           
        }
        static List<long> EratosthenesSieve(long max_value)
        {
            List<long> primes = new List<long>();

            BitArray sieve = new BitArray(max_value + 1, true);
            for (int n = 2; n <= max_value; ++n)
            {
                if (sieve[n])  // se true, allora n è primo
                {
                    primes.Add(n);
                    for (int mult_n = n + n; mult_n <= max_value; mult_n += n)  // genera tutti i multipli di n, che vanno marcati come non-primi
                    {
                        sieve[mult_n] = false;
                    }
                }
            }

            return primes;
        }
        static void Main(string[] args)
        {
            List<long> primes = EratosthenesSieve(64_000_000);

            foreach (long n in primes)
                Console.Write($"{n}, ");
            Console.WriteLine();
        }
    }
}
