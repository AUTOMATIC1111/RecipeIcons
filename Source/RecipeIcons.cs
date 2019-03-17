using Harmony;
using System.Reflection;
using Verse;

namespace RecipeIcons
{
    [StaticConstructorOnStartup]
    public class RecipeIcons
    {
        static RecipeIcons()
        {
            var harmony = HarmonyInstance.Create("com.github.automatic1111.recipeicons");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [StaticConstructorOnStartup]
    public class Testing
    {
        static Testing()
        {
            Verse.Log.Message("Testing");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\test.txt"))
            {
                file.WriteLine("Testing");
            }
        }
    }

}
