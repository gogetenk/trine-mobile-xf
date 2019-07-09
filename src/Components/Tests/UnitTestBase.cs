using AutoMapper;
using Moq;
using Prism.Logging;
using Prism.Navigation;
using Prism.Services;
using Trine.Mobile.Components.Builders;

namespace Trine.Mobile.Components.Tests
{
    public abstract class UnitTestBase
    {
        protected readonly IMapper _mapper;
        protected readonly Mock<ILogger> _logger;
        protected readonly Mock<IPageDialogService> _pageDialogService;
        protected readonly Mock<INavigationService> _navigationService;

        public UnitTestBase()
        {
            _mapper = BuildAutoMapper();
            _logger = new Mock<ILogger>();
            _pageDialogService = new Mock<IPageDialogService>();
            _navigationService = new Mock<INavigationService>();
        }

        protected IMapper BuildAutoMapper()
        {
            var mapper = new MapperBuilder().CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            return mapper;
        }
    }
}
