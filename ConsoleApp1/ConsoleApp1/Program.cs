A ab = new B();
ab.PrintName();

B bc = new C();
bc.PrintName();

A ac = new C();
ac.PrintName();


C c = new C();
((B)c).PrintName();




//C cb = new B();
//
//b.PrintName();



abstract class A
{
    public A()
    {
        Console.WriteLine("ctor classA");
    }


    public virtual string Name => "Class A";


    public void PrintName()
    {
        Console.WriteLine($"Name: {Name}");
    }
}

class B : A
{
    static B()
    {
        Console.WriteLine("static ctor classB");
    }

    public B()
    {
        Console.WriteLine("ctor classB");
    }

    public override string Name => "Class B";
}

class C : B
{
    public C()
    {
        Console.WriteLine("ctor classC");
    }

    public override string Name => "Class C";
}

class Event
{
    public int TypeId     {         get; set;     }

    public DateTime Date { get; set; }
}

//Dictionary<Event, int> dict = new Dictionary<Event, int>();
