using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Microsoft.VisualBasic;

namespace Planer_Dnia;

public partial class MainWindow : Window
{ 
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void AddTaskToGrid(Task task)
    {
        int newRowIndex = TaskGrid.RowDefinitions.Count;

        if (newRowIndex >= TaskGrid.RowDefinitions.Count)
        {
            TaskGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }

        var taskNameTextBlock = new TextBlock
        {
            Text = task.Name,
            Margin = new Thickness(15, 5, 5, 5)
        };

        var taskTypeComboBox = new ComboBox // lista do zmiany typu zadania
        {
            SelectedItem = task.Type,
            Margin = new Thickness(15, 5, 5, 5)
        };
        
        // Dodanie każdego elementu do listy (tej do zmiany typu zadania)
        foreach (var item in ComboBoxTaskType.Items)
        {
            taskTypeComboBox.Items.Add(item);
        }

        var finishedCheckBox = new CheckBox
        {
            Content = "Zakończone",
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(15, 5, 5, 5)
        };

        var deleteButton = new Button
        {
            Content = "Usuń zadanie",
            Background = Brushes.Firebrick,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(15, 5, 5, 5),
        };

        deleteButton.Click += (sender, e) =>
        {
            TaskGrid.Children.Remove(taskNameTextBlock);
            TaskGrid.Children.Remove(taskTypeComboBox);
            TaskGrid.Children.Remove(finishedCheckBox);
            TaskGrid.Children.Remove(deleteButton);
            UpdateTaskSummary();
        };
        
        // Dodanie kontrolek do Grid
        TaskGrid.Children.Add(taskNameTextBlock);
        TaskGrid.Children.Add(taskTypeComboBox);
        TaskGrid.Children.Add(finishedCheckBox);
        TaskGrid.Children.Add(deleteButton);
        
        // Ustawienie wiersza dla kontrolek
        Grid.SetRow(taskNameTextBlock, newRowIndex);
        Grid.SetRow(taskTypeComboBox, newRowIndex);
        Grid.SetRow(finishedCheckBox, newRowIndex);
        Grid.SetRow(deleteButton, newRowIndex);
        
        // Ustawienie kolumn
        Grid.SetColumn(taskNameTextBlock, 0);
        Grid.SetColumn(taskTypeComboBox, 1);
        Grid.SetColumn(finishedCheckBox, 2);
        Grid.SetColumn(deleteButton, 2);
    }

    private void UpdateTaskSummary()
    {
        
    }
    
    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        string TaskName = TextBoxTaskName.Text;
        string TaskType = (ComboBoxTaskType.SelectedItem as ComboBoxItem)?.Content.ToString();

        if (string.IsNullOrWhiteSpace(TaskName) || string.IsNullOrWhiteSpace(TaskType))
        {
            TextBlockTaskNumber.Text = "Wypełnij puste pola.";
            return;
        }

        Task newTask = new Task()
        {
            Name = TaskName,
            Type = TaskType
        };

        AddTaskToGrid(newTask); // Nie działa?
        
        TextBoxTaskName.Clear();
        ComboBoxTaskType.SelectedIndex = -1;
    }
}