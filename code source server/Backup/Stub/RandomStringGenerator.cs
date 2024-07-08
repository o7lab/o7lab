namespace Stub
{
    using System;

    public class RandomStringGenerator
    {
        private const string LOWERCASE = "abcdefghijklmnopqrstuvwxyz";
        private const string NUMBERS = "0123456789";
        private Random r;
        private const string UPPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public RandomStringGenerator()
        {
            this.r = new Random();
        }

        public RandomStringGenerator(int seed)
        {
            this.r = new Random(seed);
        }

        public virtual string NextString(int length)
        {
            return this.NextString(length, true, true, true);
        }

        public virtual string NextString(int length, bool lowerCase, bool upperCase, bool numbers)
        {
            char[] chArray = new char[length];
            string str = string.Empty;
            if (lowerCase)
            {
                str = str + "abcdefghijklmnopqrstuvwxyz";
            }
            if (upperCase)
            {
                str = str + "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }
            if (numbers)
            {
                str = str + "0123456789";
            }
            for (int i = 0; i < chArray.Length; i++)
            {
                int num2 = this.r.Next(0, str.Length);
                chArray[i] = str[num2];
            }
            return new string(chArray);
        }
    }
}

