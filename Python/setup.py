from distutils.core import setup
import py2exe

setup(
    console = [
        {
            "script":"PPE Charts.py", 
            "icon_resources": [(0,"tkplot.ico")]
        } 
    ]
    )