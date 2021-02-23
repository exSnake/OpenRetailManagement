// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="exSnake Production">
//   All right reserved
// </copyright>
// <summary>
//   Defines the Bootstrapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ORMDesktopUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    using Caliburn.Micro;

    using ORMDesktopUI.ViewModels;

    /// <summary>
    /// The bootStrap.
    /// </summary>
    public class Bootstrapper : BootstrapperBase
    {
        /// <summary>
        /// Our container it's going to handle instantiation of our classes.
        /// </summary>
        private SimpleContainer container = new SimpleContainer();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public Bootstrapper()
        {
            this.Initialize();
        }

        /// <summary>
        /// Whenever you ask for a instance of SimpleContainer it will return the container itself.
        /// </summary>
        protected override void Configure()
        {
            this.container.Instance(this.container);
            
            this.container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();

            // Get every type in out entire applications that is a class and his name ends with ViewModel, take that
            // list and add it to the container so every request he will create a new instance usually registerPerRequest
            // accepts a Interface,Name,Class for Testing Purpose instead of what we did, Class,Name,Class
            this.GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModel => this.container.RegisterPerRequest(viewModel, viewModel.ToString(), viewModel));
        }

        /// <summary>
        /// The on startup i want you to launch ShellViewModel as our base view
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            this.DisplayRootViewFor<ShellViewModel>();
        }

        /// <summary>
        /// If we pass a type and a name we get that instance, find the location and create a new instance using the container.
        /// </summary>
        /// <param name="service"> The service. </param>
        /// <param name="key"> The key. </param>
        /// <returns> The <see cref="object"/>. </returns>
        protected override object GetInstance(Type service, string key)
        {
            return this.container.GetInstance(service, key);
        }

        /// <summary>
        /// The get all instances, same as before but all.
        /// </summary>
        /// <param name="service"> The service. </param>
        /// <returns> The <see cref="IEnumerable"/>. </returns>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return this.container.GetAllInstances(service);
        }

        /// <summary>
        /// The build up.
        /// </summary>
        /// <param name="instance"> The instance. </param>
        protected override void BuildUp(object instance)
        {
            this.container.BuildUp(instance);
        }
    }
}
