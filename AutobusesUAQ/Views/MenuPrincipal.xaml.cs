using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AutobusesUAQ.Views
{
    public partial class MenuPrincipal : MasterDetailPage
    {
        public MenuPrincipal()
        {
            InitializeComponent();
            inicio();
        }

        void inicio()
        {
            List<Models.Menu> menu = new List<Models.Menu>{//Le cambie Menu a Models.Menu porque al ejecutarlo en iOS manda error de ambiguo
                new Models.Menu { id= 1, titulo = "Inicio"/*, detalle = "Regresa a la página de inicio."*/, icono = "inicio.png"},
                new Models.Menu { id= 2, titulo = "Choferes"/*, detalle = "Regresa a la página de super."*/, icono = "carrito.png"},
                //new Models.Menu { id= 3, titulo = "Departamentos"/*, detalle = "Regresa a la página de departamentos."*/, icono = "categorias.png"},
                new Models.Menu { id= 4, titulo = "Acerca de"/*, detalle = "Regresa a la página de acerca de."*/, icono = "acerca.png"},
                new Models.Menu { id= 5, titulo = "Salir"/*, detalle = "Cerrar la aplicación."*/, icono = "salir.png"},
                //new Models.Menu { id= 6, titulo = "Ingresar/Registrarse"/*, detalle = "Cerrar la aplicación."*/, icono = "acerca.png"}
            };
            ListaMenu.ItemsSource = menu;
            Detail = new NavigationPage(new Inicio());//Se cambia para que sea la cartelera la primera en cargar
        }

        public async void ListaMenu_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var menu = e.SelectedItem as Models.Menu;
            if (menu != null)
            {
                if (menu.id == 1)//Inicio 
                {
                    Detail = new NavigationPage(new Inicio());
                    IsPresented = false;//Para que el menu desaparesca cuando se le haga click
                }
                if (menu.id == 2)//Choferes
                {
                    IsPresented = false;//Para que el menu desaparesca cuando se le haga click
                    Detail = new NavigationPage(new ChoferesView());
                }
                if (menu.id == 3)
                {
                    IsPresented = false;//Para que el menu desaparesca cuando se le haga click
                    //Detail = new NavigationPage(new DepartamentosView());
                }
                if (menu.id == 4)//Acerca de
                {
                    IsPresented = false;//Para que el menu desaparesca cuando se le haga click
                    Detail = new NavigationPage(new AcercaDe());
                }
                if (menu.id == 5)//Salir
                {
                    string mensaje = "¿Deseas cerrar la aplicación?";
                    var resp = await this.DisplayAlert("Confirmación", mensaje, "SI", "NO");
                    if (resp)
                    {
                        System.Environment.Exit(0);
                    }
                }
                ListaMenu.SelectedItem = null;//Para que automaticamente se deseleccione el elemento
            }
        }
    }
}
