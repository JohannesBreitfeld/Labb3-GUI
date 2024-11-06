# Labb3-GUI
Third and final assignment of the C# programming course in the .NET development program at IT-Högskolan.
The task is to create a quiz app in WPF using the MVVM architectural pattern.

## Implementation of MVVM
I have tried my best to adhere strictly to the MVVM pattern and separate the Viewmodel from the View and the Model from the Viewmodel. 
There is no logic in the code-behinds, and I have tried to minimize the code in them. However, I do have one opening/closing event that triggers commands in the ViewModel, and the code-behind of each dialog contains an OK/Cancel button click event.

### Use of Dialog Windows
There seem to be thousands of ways and no real best practice when implementing dialog windows in the MVVM pattern.
After much consideration, I chose to go with what's called a dialog service. 
I created a separate class, implementing an interface, that handles the communication with the dialog window through bindings.
Then I inject the class handling the dialog when creating a new instance of the ViewModel. 
That way, the ViewModel is only dependent on the abstraction of a class implementing the interface, with one method returning an instance of the QuestionPackViewModel class.
If I decided to write automated tests for the app, I would only have to create a mock class implementing the interface and inject that instead of the dialog service class.