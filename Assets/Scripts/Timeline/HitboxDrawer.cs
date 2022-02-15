using UnityEngine;

[ExecuteInEditMode]
public class HitboxDrawer : MonoBehaviour
{
    //矩形的四个顶点
    public Vector2[] points = new Vector2[4];

    // When added to an object, draws colored rays from the
    // transform position.
    public int lineCount = 100;
    public float radius = 3.0f;

    //自定义材质
    static Material lineMaterial;
    static void CreateLineMaterial()
    {
        if (!lineMaterial)//若没有就创建
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");//后面的是Shader名（应该是在项目中找）
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);

        }
    }


    //在相机渲染场景后调用（在OnPostRender()前执行)
    public void OnRenderObject()
    {
        CreateLineMaterial();
        // Apply the line material
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        lineMaterial.SetPass(0);
        //GL.LoadOrtho();
        GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.QUADS);

        var color = Color.cyan;
        color.a = 0.5f;//半透明效果(并不用用到Shader)
        GL.Color(color);

        //用for循环实时（在Editor模式下）绘制（未实现）
        GL.Vertex3(points[0].x, points[0].y, 0);
        GL.Vertex3(points[1].x, points[1].y, 0);
        GL.Vertex3(points[2].x, points[2].y, 0);
        GL.Vertex3(points[3].x, points[3].y, 0);


        GL.End();
        GL.PopMatrix();


        GL.PushMatrix();
        // Set transformation matrix for drawing to
        // match our transform
        GL.MultMatrix(transform.localToWorldMatrix);

        // Draw lines
        GL.Begin(GL.LINES);
   

        GL.End();
        GL.PopMatrix();
    }

    Material mat;
    // Draw red a rombus on the screen
    // and also draw a small cyan Quad in the left corner
    //在相机渲染场景后 Unity 调用的事件函数
    void OnPostRender()
    {
        if (!lineMaterial)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        GL.PushMatrix();
        //激活给定的 通道 以进行渲染https://docs.unity3d.com/cn/current/ScriptReference/Material.SetPass.html
        mat.SetPass(0);
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.Color(Color.red);
        GL.Vertex3(0, 0.5f, 0);
        GL.Vertex3(0.5f, 1, 0);
        GL.Vertex3(1, 0.5f, 0);
        GL.Vertex3(0.5f, 0, 0);

        GL.Color(Color.cyan);//颜色名
        GL.Vertex3(0, 0, 0);
        GL.Vertex3(0, 0.25f, 0);
        GL.Vertex3(0.25f, 0.25f, 0);
        GL.Vertex3(0.25f, 0, 0);
        GL.End();
        GL.PopMatrix();
    }

}