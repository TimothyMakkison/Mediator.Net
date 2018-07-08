﻿using System.Threading.Tasks;
using Mediator.Net.IoCTestUtil;
using Mediator.Net.IoCTestUtil.Messages;
using Mediator.Net.IoCTestUtil.Middlewares;
using Mediator.Net.IoCTestUtil.Services;
using Shouldly;
using StructureMap;
using TestStack.BDDfy;
using Xunit;

namespace Mediator.Net.StructureMap.Test.Tests
{

    public class TestRequestHandlerWithDependancyInjection : TestBase
    {
        private IContainer _container = null;
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
            _container.Configure(x =>
            {
                x.ForConcreteType<SimpleService>();
                x.ForConcreteType<AnotherSimpleService>();
            });
            _container.Configure(mediaBuilder);
        }

        Task WhenARequestIsSent()
        {
            _mediator = _container.GetInstance<IMediator>();
            _task = _mediator.RequestAsync<SimpleRequest, SimpleResponse>(new SimpleRequest("Hello"));
            return _task;
        }

        Task ThenTheRequestShouldReachItsHandler()
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
