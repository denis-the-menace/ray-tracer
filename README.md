<h1>Ray Tracer with .NET Core</h1>

<h2>How to compile and run</h2>
Install .NET SDK<br>
Run the command<br>
<code>dotnet run [JSON input file] [output file] Program.cs</code>

<h2>Results</h2>
![scene1](https://github.com/denis-the-menace/ray-tracer/assets/101937972/b354a7a4-426b-4c70-b154-7883c097b026)
![scene2](https://github.com/denis-the-menace/ray-tracer/assets/101937972/aa7992b7-8ac3-460b-a031-3309e9a61f84)
![scene3](https://github.com/denis-the-menace/ray-tracer/assets/101937972/dac879c2-f153-41be-8a03-c76528e082ea)

<h2>Example input file</h2>
Feel free to change SceneParser.cs for different inputs.<br>
<code>{
	"perspectivecamera" : {
		"center" : [0, 0.5, 5],
		"direction" : [0, -0.1, -1],
		"up" : [0, 1, 0],
		"angle" : 30
	},
	"background" : {
		"color" : [0.2, 0.1, 0.6],
		"ambient" : [0.2, 0.2, 0.2]
	},
	"group" : [
		{ 
			"sphere" : {
				"center" : [0.3, 0.0, -1],
				"radius" : 1.0,
				"material" : 0
			}
		},
	],
	"lights" : [
		{
			"directionalLight" : {
				"direction" : [0.5, -1, 0],
				"color" : [0.8, 0.8, 0.8]
			}
		}
	],
	"materials" : [
		{
			"phongMaterial" : {
				"diffuseColor" : [0.1, 0.1, 0.1],
				"specularColor" : [1, 1, 1],
				"exponent" : 50,
				"transparentColor" : [0, 0, 0],
				"reflectiveColor" : [0.9, 0.9, 0.9],
				"indexOfRefraction" : 1
			}
		},
	]
}
</code>
