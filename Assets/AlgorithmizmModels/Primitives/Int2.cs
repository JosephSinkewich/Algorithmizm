using System;

namespace AlgorithmizmModels.Primitives
{
    [Serializable]
    public class Int2
    {
        public int x;
        public int y;

        public Int2()
        {
            x = 0;
            y = 0;
        }

        public Int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Int2 other)
            {
                return x == other.x
                    && y == other.y;
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int tmp = y + ((x + 1) / 2);
                int result = x + tmp * tmp;

                return result;
            }
        }

        public override string ToString()
        {
            return $"[x:{x}, y:{y}]";
        }

        public Int2 Clone()
        {
            return new Int2(x, y);
        }

        public static Int2 operator +(Int2 left, Int2 right)
        {
            return new Int2(
                left.x + right.x,
                left.y + right.y);
        }

        public static Int2 operator -(Int2 left, Int2 right)
        {
            return new Int2(
                left.x - right.x,
                left.y - right.y);
        }

        public static bool operator ==(Int2 left, Int2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Int2 left, Int2 right)
        {
            return !left.Equals(right);
        }
    }
}
