using System.Reflection;
using AutoFixture.Kernel;

namespace Tests.Primitives
{
    internal class InlineDataSpecimenBuilder : ISpecimenBuilder
    {
        private readonly object[] values;
        private int valueIndex;

        public InlineDataSpecimenBuilder(object[] values) => this.values = values;

        public object Create(object request, ISpecimenContext context)
        {
            if (request is ParameterInfo && valueIndex < values.Length) return values[valueIndex++];
            return new NoSpecimen();
        }
    }
}
