using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TaxMeApp.Helpers
{
    class RelayCommand : ICommand
    {

        /*
          
                DO NOT MODIFY
                Found this somewhere either on Windows Docs or Stack Overflow
                Works absolutely perfectly
                Will provide small tutorial in comments below this class
          
        */

        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {

            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute;

        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            execute(parameter ?? "<N/A>");
        }

    }
}


/*
 
    How to use buttons with dynamic binding:

    in XAML:

    // The binding goes to COMMAND. Do not try to bind this to "Click" because it will give an error. ( Ask me how I know :) )
    // Try to keep the naming format of ______Command
    <Button Command="{Binding WhateverCommand}" Content="Whatever"/>

    in ViewModel:

    // Create a public uninstantiated class variable of type ICommand with the same name as the binding above
    public ICommand WhateverCommand { get; set; }

    in constructor of VM:

    // Instantiate the command you declared above
    WhateverCommand = new RelayCommand(o => whateverClick(""));
    // For different buttons, the blank parts are the only ones that should change:
    // With "_____Click("")" being the function you declare below
    _____Command = new RelayCommand(o => _____Click("_____"));

    as a class function in the VM:

    // Try to keep the naming format for ____Click for the function
    // For the argument of the function, you can pass it variables if there's a need for code reuse. I'll include example down below
    private void whateverClick(object sender)
    {
        // button logic
    }
    // Otherwise, the only thing that should change in the function declaration is the *function name* even if you don't use args
    

    


    // Example of passing in values for the args for code reuse:
    
    // Four buttons, one click logic (specific case passed in to arg)
    LapSortCommand = new RelayCommand(o => sortComparisonFiles("lap"));
    FilenameSortCommand = new RelayCommand(o => sortComparisonFiles("filename"));
    CarSortCommand = new RelayCommand(o => sortComparisonFiles("car"));
    TrackSortCommand = new RelayCommand(o => sortComparisonFiles("track"));

    // Not neccessarily THE way, but it is a way that I found easy to do:
    private void sortComparisonFiles(object sender)
    {
        // cast the args to string to use in the switch
        string type = (string)sender;

        switch (type)
        {
            case "lap":
                // logic
                break;
            case "filename":
                // logic
                break;
            case "car":
                // logic
                break;
            case "track":
                // logic
                break;
            default:
                Trace.WriteLine(type + " not implemented yet.");
                break;
        }
    }





*/
