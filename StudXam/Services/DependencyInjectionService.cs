using Autofac;
using StudXam.DI;
using System;
using System.Collections.Generic;

namespace StudXam.Services
{
    public class DependencyInjectionService : IDependencyInjectionService
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

        public void RegisterPageInstance<P>(Func<P> func)
        {
            containerBuilder.RegisterInstance(func);
        }

        public T Resolve<T>()
        {
            return DiContainer.Resolve<T>();
        }

        public void RegisterWithParameterType<T, P>(DiInstanceTypeEnum instanceTypeEnum = DiInstanceTypeEnum.NewInstancePerRequest)
        {
            switch (instanceTypeEnum)
            {
                case DiInstanceTypeEnum.SingleInstance:
                    containerBuilder.RegisterType<T>().WithParameter(
                        new TypedParameter(typeof(P), null)
                        ).SingleInstance();
                    break;

                case DiInstanceTypeEnum.InstancePerLifetimeScope:
                    containerBuilder.RegisterType<T>().WithParameter(
                        new TypedParameter(typeof(P), null)
                        ).InstancePerLifetimeScope();
                    break;

                case DiInstanceTypeEnum.InstancePerLifetimeScopeWithSingleInstance:
                    containerBuilder.RegisterType<T>().WithParameter(
                        new TypedParameter(typeof(P), null)
                        ).InstancePerLifetimeScope().SingleInstance();
                    break;

                case DiInstanceTypeEnum.InstancePerRequest:
                    containerBuilder.RegisterType<T>().WithParameter(
                        new TypedParameter(typeof(P), null)
                        ).InstancePerRequest();
                    break;

                default:
                    containerBuilder.RegisterType<T>().WithParameter(
                    new TypedParameter(typeof(P), null)
                    );
                    break;
            }
        }

        public T ResolveWithParameter<T, P>(P param)
        {
            var typedParams = new List<TypedParameter> { new TypedParameter(typeof(P), param) };
            return DiContainer.Resolve<T>(typedParams);
        }

        public T ResolvePageWithVmParameter<T, V, P>(P param)
        {
            var typedParams = new List<TypedParameter> { new TypedParameter(typeof(P), param) };
            using (var lifetime = DiContainer.BeginLifetimeScope())
            {
                var vm = lifetime.Resolve<V>(typedParams);
                return lifetime.Resolve<T>();
            }
        }
    }
}