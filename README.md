# Coligo.net
Small MVVM Framework for .Net WPF applications.

Coligo.Net was created after a small period of using Caliburn.Micro, and I give all credit to the Caliburn team
for providing me with the inspiration and some ideas on how to implement my own MVVM Framework.

I take nothing away from Caliburn, but just were some places where I felt that it just did not provide me with
what I needed to get the job done with various WPF projects.

What I wanted to acheive with Coligo **(latin for 'Connect/Bind')** was a simple and easy way to Bind your ViewModels to
your UI elements, in an easy manner and with a little more 'power' than what regular WPF binding allows.

The Caliburn 'conventions' were also an inspiration for my Coligo conventions, just implemented slightly differently, with
it's own XAML markup mechanism.

Now, since Coligo is only in it's earliest stages, there will of course be many changes (hopefully improvements) to the way
it functions, but so far it has done a pretty good job at solving some of the issues I came across with Caliburn.

For now, the Coligo assemblies have been developed to be used in both a **Desktop WPF** environment and a **WinRT** environment, so
in the case of a Windows Store application or a Windows Phone 8+ application, the libraries have beed developed to utilise
the WinRT BCL, which annoyingly, differs quite alot from the regular non-WinRT BCL!

Please keep tuned in formore updates, I am just writing this quickly to have some form of README in place.

**Coligo is a CONVENTIONS based MVVM Framework** with it's own small IOC Container engine to handle dynamic ViewModel creation
and loading on demand.

**PROJECT CONVENTIONS:**
First, your Visual Studio project *should* follow this FOLDER structure: (this is NOT required)
- **project-folder**
    - **ViewModels**           - This folder contains all your ViewModel classes.
    - **Views**                - This folder contains your UI views.

**BINDING CONVENTIONS**
Coligo is after all a Binding Framework at it's heart. It allows you to make use of Bindings to delegate runtime logic to your ViewModels, and thus remove the need
to have any code sitting in your code-behind files. Code-behind is not really a *bad* thing, and originally **was** the only way to place runtime logic that was triggered
by UI events.

Of course, the **MVVM** pattern attempts to describe a means of having a ViewModel to be the 'middle-man' between your UI (Views) and your Data/Data-model. Even though the
mention of **code-behind** should not really be found in MVVM pattern descriptions, what MVVM frameworks attempt to do is to provide the **plumbing** to delegate all
UI fired events and binding logic straight to your ViewModel.

Remember - Your ViewModel is the workhorse, it should exist **purely** to satisfy the needs of your **view**, and whatever is required to provide the data for the view,
which generally means interacting with your domain data-model to retrieve such data.

**So how does Coligo do this?** If using Coligo with WPF (Windows, WinRT/WindowsPhone 8.1), the Binding conventions are simply Attached properties in which you provide
binding statements which Coligo Engine inspects at runtime and plumbs all the required elements to the ViewModel elements (Properties and Action/Methods).

The **conventions** are where Coligo implicitly <i>hooks up</i> properties and events from **UI elemets (depending on the type of element)** to various properties and methods
on your ViewModel.

**LETS BEGIN WITH AN EXAMPLE:**

Below is an example MainWindow.xaml file, which is in a **Views** folder in a Windows WPF application. This file is a simple UI that has a TabControl as the
main UI component, with 2 Tabs, each tab refers to 2 other XAML files, which a **UserControls** that is required if you want be able to **see** the content
of the tab in the VisualStudio designer at design time.

	<window x:class="WpfTabsSample.MainWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		**xmlns:b="clr-namespace:Coligo.Platform.Binder;assembly=Coligo.Platform.WPF"**
		xmlns:views="clr-namespace:WpfTabsSample.Views"
		**b:Binder.Model="MainViewModel"**
		title="{Binding Name}" height="400" width="400">
	    <Grid>
		<TabControl x:name="MyTabs">

		    <TabItem header="{Binding Name}" **b:Binder.Model="Tab1ViewModel"**>
			<views:tab1view />
		    </TabItem>

		    <TabItem header="{Binding Name}" b:Binder.Model="Tab2ViewModel">
			<views:tab2view />
		    </TabItem>

		</TabControl>
	    </Grid>
	</Window>

Notice **xmlns:b="clr-namespace:Coligo.Platform.Binder;assembly=Coligo.Platform.WPF"** and **b:Binder.Model="MainViewModel"**.
The first sets up the xaml-namespace to the required Coligo namespace and assembly. If this were a WindowsPhome 8.1 project, the assembly
would be **Coligo.Platform.WP81**.

The second highlighed element, **b:Binder.Model="MainViewModel"**, is calling the staic Binder class, telling Coligo which ViewModel to bind
to this UI Element.

The TabControl has 2 Tabs, with a Coligo Binding statement which binds a Tab1ViewModel instance to the UI Element (TabItem).

Also notice that you can mix standard xaml binding statements (**header="{Binding Name}"**) along with Coligo binding.

**Here is MainViewModel.cs (very simple example):**

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Coligo.Platform.Binder;
	using Coligo.Core;
	using Coligo.Platform;

	namespace WpfTabsSample.ViewModels
	{
	    public class MainViewModel : **BaseViewModel**
	    {

		/// <summary>
		/// Public property
		/// </summary>
		public string Name
		{
		    get { return "Your MainWindow"; }
		}

	    }
	}


**Tab1View.xaml**

	<UserControl x:class="WpfTabsSample.Views.Tab1View"
		     xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		     xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		     xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
		     xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
		     **xmlns:b="clr-namespace:Coligo.Platform.Binder;assembly=Coligo.Platform.WPF"**
		     mc:ignorable="d">

	    <StackPanel orientation="Vertical">
		<TextBlock **b:Binder.Bind="$this:PostCode"** margin="5" />
		<TextBlock **b:Binder.Bind="$this:SelectedItem"** margin="5" />

		<TextBox **Name="PostCode"** **b:Binder.Bind="$this"** margin="5" />

		<TreeView height="100" **b:Binder.Bind="ItemsSource:Items;SelectedItem:SelectedTreeItem"** margin="5">

		</TreeView>

		<ListView **b:Binder.Bind="$this;SelectedItem:SelectedItem;Items:Items"** margin="5" />

		<CheckBox x:Name="CanCheckPostCode" Content="Can Submit Postcode" margin="5" />

		<Button Name="CheckPostCode" Content="Submit PostCode" b:Binder.Bind="$this" width="100" margin="5" />

		<Button Name="AddItemAction" b:Binder.Bind="$this" Content="Add Item" width="100" margin="5" />

		<Label>
		    <Hyperlink **b:Binder.Action="Click:HelpAction"**>
			<Label Content="Help" />
		    </Hyperlink>
		</Label>
	    </StackPanel>

	</UserControl>

Use of **Binder**...

To hook-up a ViewModel class with an entire UI Element...
**b:Binder.Model="name-of-your-viewmodel-class"**

To bind a UI Element with implicit conventions, or to bind UI Element properties to ViewModel properties...
**b:Binder.Bind="([$this]|[$this:vm-property-name]|[ui-element-property:vm-property]);"**

To bind UI Element **events** to ViewModel 'Actions', or methods...
**b:Binder.Action="ui-event-name:vm-void-method"**


**More documentation and samples to come soon!**
