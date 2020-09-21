namespace StudXam.DI
{
    public interface IDependencyInjectionService
    {
        void RegisterType<T>(DiInstanceTypeEnum diInstanceTypeEnum = DiInstanceTypeEnum.NewInstancePerRequest);

        void RegisterType<T, D>(bool isSingleton = false);

        T Resolve<T>();

        void Build();
    }
}