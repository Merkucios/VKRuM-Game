namespace WreckingBall.Factory
{
    public interface IFactory<T>
    {
        T Create();
    }
}