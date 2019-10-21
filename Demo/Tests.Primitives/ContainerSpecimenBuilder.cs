using System;
using System.Reflection;
using AutoFixture.Kernel;
using SimpleInjector;

namespace Tests.Primitives
{
    public class ContainerSpecimenBuilder : ISpecimenBuilder
    {
        private readonly Container container;

        internal ContainerSpecimenBuilder(Container container) => this.container = container;

        public object Create(object request, ISpecimenContext context)
        {
            switch (request)
            {
                case ParameterInfo parameterInfo:
                    return Create(parameterInfo.ParameterType);
                case SeededRequest seededRequest:
                    if (seededRequest.Request is Type type) return Create(type);
                    break;
            }

            return new NoSpecimen();
        }

        private object Create(Type serviceType)
        {
            if (serviceType.IsInterface || IsInterfaceArray(serviceType) || IsFactory(serviceType))
            {
                var producer = container.GetRegistration(serviceType);
                if (producer != null) return producer.GetInstance();
            }

            return new NoSpecimen();
        }

        private static bool IsInterfaceArray(Type serviceType) =>
            serviceType.IsArray && serviceType.GetElementType()?.IsInterface == true;
        private static bool IsFactory(Type serviceType) =>
            serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(Func<>);
    }
}
