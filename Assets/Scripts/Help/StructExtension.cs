namespace Help
{
    public static class StructExtension
    {
        public static T Clone<T> ( this T val ) where T : struct => val;
    }
}
