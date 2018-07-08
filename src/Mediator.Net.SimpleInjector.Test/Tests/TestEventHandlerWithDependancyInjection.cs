﻿using System.Threading.Tasks;
using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.IoCTestUtil.Middlewares;
using Shouldly;
using SimpleInjector;
using SimpleInjector.Extensions.LifetimeScoping;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.SimpleInjector.Test.Tests
{

    public class TestEventHandlerWithDependancyInjection : TestBase
    {
        private Container _container = null;
        private IMediator _mediator;
        private Task _task;
 
        void GivenAContainer()
        {
            ClearBinding();
            var mediaBuilder = new MediatorBuilder();
            mediaBuilder.RegisterHandlers(TestUtilAssembly.Assembly)
                .ConfigureCommandReceivePipe(x =>
                {
                    x.UseSimpleMiddleware();
                });
            _container = new Container();
            _container.Options.DefaultScopedLifestyle = new LifetimeScopeLifestyle();
            _container.RegisterMediator(mediaBuilder);
        }

        Task WhenACommandIsSent()
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                _mediator = scope.GetInstance<IMediator>();
            _task = _mediator.PublishAsync(new SimpleEvent());
            }
            return Task.FromResult(0);
        }

        Task ThenTheEventShouldReachItsHandler()
        {
            _task.Status.ShouldBe(TaskStatus.RanToCompletion);
            return Task.FromResult(0);
        }

        [Fact]
        public void Run()
        {
            this.BDDfy();
        }
    }
}
