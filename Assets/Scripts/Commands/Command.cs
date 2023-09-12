using System.Threading.Tasks;

public abstract class Command
{
    private bool _isExecuting = false;
    public bool isExecuting
    {
        get => isExecuting;
    }

    public async void Execute()
    {
        _isExecuting = true;
        
        await AsyncExecuter();
        
        _isExecuting = false;
    }
    protected abstract Task AsyncExecuter();

}
