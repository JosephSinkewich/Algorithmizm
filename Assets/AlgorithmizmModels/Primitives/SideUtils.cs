namespace AlgorithmizmModels.Primitives
{
    public static class SideUtils
    {
        private const int COUNT_SIDES = 4;

        public static Int2 ToInt2(this Side side)
        {
            switch (side)
            {
                case Side.Bottom:
                    return new Int2(0, -1);
                case Side.Left:
                    return new Int2(-1, 0);
                case Side.Right:
                    return new Int2(1, 0);
                case Side.Top:
                    return new Int2(0, 1);
                default:
                    return new Int2(0, 0);
            }
        }

        public static Side Rotate(this Side side, int rotation)
        {
            int result = ((int)side + rotation) % COUNT_SIDES;
            if (result < 0)
            {
                result += COUNT_SIDES;
            }
            return (Side)result;
        }

        public static float ToAngle(this Side side)
        {
            return (int)side * 90;
        }
    }
}
