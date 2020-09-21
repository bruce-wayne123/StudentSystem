using Autofac;

namespace StudXam.DI
{
    internal class DependencyInjectionService : IDependencyInjectionService
    {
        private IContainer DiContainer;
        private ContainerBuilder containerBuilder;

        public DependencyInjectionService()
        {
            this.containerBuilder = new ContainerBuilder();
        }

        public void Build()
        {
            DiContainer = this.containerBuilder.Build();
        }

        public void RegisterType<T, D>(bool isSingleton)
        {
            if (isSingleton)
            {
                this.containerBuilder.RegisterType<T>().As<D>().SingleInstance();
            }
            else
            {
                this.containerBuilder.RegisterType<T>().As<D>();
            }
        }

        public void RegisterType<T>(DiInstanceTypeEnum diInstanceTypeEnum)
        {
            switch (diInstanceTypeEnum)
            {
                case DiInstanceTypeEnum.SingleInstance:
                    this.containerBuilder.RegisterType<T>().SingleInstance();
                    break;

                case DiInstanceTypeEnum.InstancePerLifetimeScope:
                    this.containerBuilder.RegisterType<T>().InstancePerLifetimeScope();
                    break;

                case DiInstanceTypeEnum.InstancePerLifetimeScopeWithSingleInstance:
                    this.containerBuilder.RegisterType<T>().InstancePerLifetimeScope().SingleInstance();
                    break;

                case DiInstanceTypeEnum.InstancePerRequest:
                    this.containerBuilder.RegisterType<T>().InstancePerRequest();
                    break;

                default:
                    this.containerBuilder.RegisterType<T>();
                    break;
            }
        }

        public T Resolve<T>()
        {
            return DiContainer.Resolve<T>();
        }
    }
}