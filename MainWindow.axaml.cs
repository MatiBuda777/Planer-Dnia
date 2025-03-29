using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        
        // Lista typów zadań
        var taskTypes = new ObservableCollection<string> { "Praca", "Hobby", "Zakupy", "Relaks", "Rozrywka", "Rodzina", "Znajomi", "Inne" };

        var taskTypeComboBox = new ComboBox // lista do zmiany typu zadania
        {
            SelectedItem = task.Type,
            Margin = new Thickness(15, 5, 5, 5),
            ItemsSource = taskTypes // Ustawienie źródła elementów
        };

        var finishedCheckBox = new CheckBox
        {
            Content = "Zakończone",
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(15, 5, 5, 5)
        };
        
        finishedCheckBox.Click += (_, _) =>  UpdateTaskSummary();

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
            TaskGrid.RowDefinitions.Remove(TaskGrid.RowDefinitions[newRowIndex]);
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
    
    private int CountCheckedTasks()
    {
        int count = 0;
        foreach (var child in TaskGrid.Children)
        {
            if (child is CheckBox { IsChecked: true })
            {
                count++;
            }
        }
        return count;
    }
    
    private void UpdateTaskSummary()
    {
        int numOfTasks = TaskGrid.RowDefinitions.Count;
        int numOfTasksFinished = CountCheckedTasks();
        TextBlockTaskNumber.Text = $"Liczba zadań: {numOfTasks}, Zakończone zadania: {numOfTasksFinished}";
    }
    
    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        string TaskName = TextBoxTaskName.Text;
        string TaskType = (ComboBoxTaskType.SelectedItem as ComboBoxItem)?.Content.ToString();
        
        if (string.IsNullOrWhiteSpace(TaskName) || string.IsNullOrWhiteSpace(TaskType))
        {
            EmptyWarningTextBlock.Text = "Wypełnij puste pola.";
            return;
        }
        EmptyWarningTextBlock.Text = "";

        Task newTask = new Task()
        {
            Name = TaskName,
            Type = TaskType
        };

        AddTaskToGrid(newTask);
        UpdateTaskSummary();
        
        TextBoxTaskName.Clear();
        ComboBoxTaskType.SelectedIndex = -1;
    }
}