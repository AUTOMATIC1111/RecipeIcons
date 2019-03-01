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
}
