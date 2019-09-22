using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using LtGt.DemoWpf.ViewModels;

namespace LtGt.DemoWpf
{
    public class Locator
    {
        public static void Init()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
    }
}