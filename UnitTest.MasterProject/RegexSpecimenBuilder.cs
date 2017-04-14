using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace UnitTest.Dk.Itu.Rlh.MasterProject
{
    public class RegexSpecimenBuilder : ISpecimenBuilder
    {
        private readonly string _memberName;
        private string _pattern;

        public RegexSpecimenBuilder(string memberName, string pattern)
        {
            _memberName = memberName;
            _pattern = pattern;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as ParameterInfo;

            if (pi == null)
            {  
                return new NoSpecimen();
            }

            if (pi.ParameterType == typeof(string) && pi.Name == _memberName)
            {
                var generator = new RegularExpressionGenerator();
                var regExRequest = new RegularExpressionRequest(_pattern);
                var result = generator.Create(regExRequest, context);
                return result.ToString();
            }

            return new NoSpecimen();
        }
    }
}