# Coligo.net
Small MVVM Framework for .Net WPF applications.

Coligo.Net was created after a small period of using Caliburn.Micro, and I give all credit to the Caliburn team
for providing me with the inspiration and some ideas on how to implement my own MVVM Framework.

I take nothing away from Caliburn, but just were some places where I felt that it just did not provide me with
what I needed to get the job done with various WPF projects.

What I wanted to acheive with Coligo <b>(latin for 'Connect/Bind')</b> was a simple and easy way to Bind your ViewModels to
your UI elements, in an easy manner and with a little more 'power' than what regular WPF binding allows.

The Caliburn 'conventions' were also an inspiration for my Coligo conventions, just implemented slightly differently, with
it's own XAML markup mechanism.

Now, since Coligo is only in it's earliest stages, there will of course be many changes (hopefully improvements) to the way
it functions, but so far it has done a pretty good job at solving some of the issues I came across with Caliburn.

For now, the Coligo assemblies have been developed to be used in both a <b>Desktop WPF</b> environment and a <b>WinRT</b> environment, so
in the case of a Windows Store application or a Windows Phone 8+ application, the libraries have beed developed to utilise
the WinRT BCL, which annoyingly, differs quite alot from the regular non-WinRT BCL!

Please keep tuned in formore updates, I am just writing this quickly to have some form of README in place.

<b>Coligo is a CONVENTIONS based MVVM Framework</b> with it's own small IOC Container engine to handle dynamic ViewModel creation
and loading on demand.

<b>PROJECT CONVENTIONS:</b>
First, your Visual Studio project *should* follow this FOLDER structure: (this is NOT required)
- <b>project-folder</b>
    - <b>ViewModels</b>           - This folder contains all your ViewModel classes.
    - <b>Views</b>                - This folder contains your UI views.

<b>BINDING CONVENTIONS</b>
Coligo is after all a Binding Framework at it's heart. It allows you to make use of Bindings to delegate runtime logic to your ViewModels, and thus remove the need
to have any code sitting in your code-behind files. Code-behind is not really a *bad* thing, and originally <b>was</b> the only way to place runtime logic that was triggered
by UI events.

Of course, the <b>MVVM</b> pattern attempts to describe a means of having a ViewModel to be the 'middle-man' between your UI (Views) and your Data/Data-model. Even though the
mention of <b>code-behind</b> should not really be found in MVVM pattern descriptions, what MVVM frameworks attempt to do is to provide the <b>plumbing</b> to delegate all
UI fired events and binding logic straight to your ViewModel.

Remember - Your ViewModel is the workhorse, it should exist <b>purely</b> to satisfy the needs of your <b>view</b>, and whatever is required to provide the data for the view,
which generally means interacting with your domain data-model to retrieve such data.

<b>So how does Coligo do this?</b> If using Coligo with WPF (Windows, WinRT/WindowsPhone 8.1), the Binding conventions are simply Attached properties in which you provide
binding statements which Coligo Engine inspects at runtime and plumbs all the required elements to the ViewModel elements (Properties and Action/Methods).

The <b>conventions</b> are where Coligo implicitly <i>hooks up</i> properties and events from <b>UI elemets (depending on the type of element)</b> to various properties and methods
on your ViewModel.

<b>LETS BEGIN WITH AN EXAMPLE:</b>

Below is an example MainWindow.xaml file, which is in a <b>Views</b> folder in a Windows WPF application. This file is a simple UI that has a TabControl as the
main UI component, with 2 Tabs, each tab refers to 2 other XAML files, which a <b>UserControls</b> that is required if you want be able to <b>see</b> the content
of the tab in the VisualStudio designer at design time.

	<window x:class="WpfTabsSample.MainWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		<b>xmlns:b="clr-namespace:Coligo.Platform.Binder;assembly=Coligo.Platform.WPF"</b>
		xmlns:views="clr-namespace:WpfTabsSample.Views"
		<b>b:Binder.Model="MainViewModel"</b>
		title="{Binding Name}" height="400" width="400">
	    <Grid>
		<TabControl x:name="MyTabs">

		    <TabItem header="{Binding Name}"<b>b:Binder.Model="Tab1ViewModel"</b>>
			<views:tab1view />
		    </TabItem>

		    <TabItem header="{Binding Name}" b:Binder.Model="Tab2ViewModel">
			<views:tab2view />
		    </TabItem>

		</TabControl>
	    </Grid>
	</Window>

Notice <b>xmlns:b="clr-namespace:Coligo.Platform.Binder;assembly=Coligo.Platform.WPF"</b> and <b>b:Binder.Model="MainViewModel"</b>.
The first sets up the xaml-namespace to the required Coligo namespace and assembly. If this were a WindowsPhome 8.1 project, the assembly
would be <b>Coligo.Platform.WP81</b>.

The second highlighed element, <b>b:Binder.Model="MainViewModel"</b>, is calling the staic Binder class, telling Coligo which ViewModel to bind
to this UI Element.

The TabControl has 2 Tabs, with a Coligo Binding statement which binds a Tab1ViewModel instance to the UI Element (TabItem).

Also notice that you can mix standard xaml binding statements (<b>header="{Binding Name}"</b>) along with Coligo binding.

<b>Here is MainViewModel.cs (very simple example):</b>

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Coligo.Platform.Binder;
	using Coligo.Core;
	using Coligo.Platform;

	namespace WpfTabsSample.ViewModels
	{
	    public class MainViewModel : BaseViewModel
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


<b>Tab1View.xaml</b>:

	<UserControl x:class="WpfTabsSample.Views.Tab1View"
		     xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		     xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		     xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
		     xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
		     <b>xmlns:b="clr-namespace:Coligo.Platform.Binder;assembly=Coligo.Platform.WPF"</b>
		     mc:ignorable="d">

	    <StackPanel orientation="Vertical">
		<TextBlock <b>b:Binder.Bind="$this:PostCode"</b> margin="5" />
		<TextBlock <b>b:Binder.Bind="$this:SelectedItem"</b> margin="5" />

		<TextBox <b>Name="PostCode"</b> <b>b:Binder.Bind="$this"</b> margin="5" />

		<TreeView height="100" <b>b:Binder.Bind="ItemsSource:Items;SelectedItem:SelectedTreeItem"</b> margin="5">

		</TreeView>

		<ListView <b>b:Binder.Bind="$this;SelectedItem:SelectedItem;Items:Items"</b> margin="5" />

		<CheckBox x:Name="CanCheckPostCode" Content="Can Submit Postcode" margin="5" />

		<Button Name="CheckPostCode" Content="Submit PostCode" b:Binder.Bind="$this" width="100" margin="5" />

		<Button Name="AddItemAction" b:Binder.Bind="$this" Content="Add Item" width="100" margin="5" />

		<Label>
		    <Hyperlink <b>b:Binder.Action="Click:HelpAction"</b>>
			<Label Content="Help" />
		    </Hyperlink>
		</Label>
	    </StackPanel>

	</UserControl>

Use of <b>Binder</b>...

To hook-up a ViewModel class with an entire UI Element...
<b>b:Binder.Model="name-of-your-viewmodel-class"</b>

To bind a UI Element with implicit conventions, or to bind UI Element properties to ViewModel properties...
<b>b:Binder.Bind="([$this]|[$this:vm-property-name]|[ui-element-property:vm-property]);"</b>

To bind UI Element <b>events</b> to ViewModel 'Actions', or methods...
<b>b:Binder.Action="ui-event-name:vm-void-method"</b>


<b>More documentation and samples to come soon!</b>
