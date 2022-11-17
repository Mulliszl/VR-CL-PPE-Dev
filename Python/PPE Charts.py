from tkinter import *
from tkinter.filedialog import askopenfilename
from matplotlib.backends.backend_tkagg import (FigureCanvasTkAgg, NavigationToolbar2Tk)
from numpy import NaN
import pandas as pd
import datetime
from matplotlib.figure import Figure
import math
import matplotlib.dates as md
import matplotlib   
import time

#Application

# Class used to create the main application 
class MainWindow:
        
        # Inititalise the window  
        def __init__(self,master):
            self.master = master
            self.master.title("PPE Plots")
            self.Scene = ''
            self.chart = ''
            self.master.iconbitmap("tkplot.ico")
            self.__mount__()

        # Add the window elements
        def __mount__(self):

            # DeEndes the size of chart figure
            self.fig = Figure(figsize=((self.master.winfo_screenwidth()/96)*0.85,(self.master.winfo_screenheight()/96)*0.75))
            
            # Add the figure to a canvas
            self.canvas = FigureCanvasTkAgg(self.fig, master = self.master)
            self.canvas.get_tk_widget().pack()

            # Add the matplolib toolbar to the canvas
            self.toolbar = NavigationToolbar2Tk(self.canvas,self.master)
            self.__menubar__()
            self.__filemenu__()
            self.__chartmenu__()
            self.master.config(menu=self.menubar)
            self.__createchekboxes__()

        # Create the menubar
        def __menubar__(self):
            self.menubar = Menu(self.master)

        # Add the menu "File" to the menubar and the command "Open"
        def __filemenu__(self):
            self.filemenu = Menu(self.menubar)
            self.filemenu.add_command(label= "Open", command=self.__readfile__)
            self.menubar.add_cascade(label="File", menu=self.filemenu)

        # Add the menu "Charts" to the menubar and all the plot commands 
        def __chartmenu__(self):
            self.chartmenu = Menu(self.menubar)
            self.chartmenu.add_command(label= "Cognitive Load", command = self.__plot_cognitive_load__)
            self.chartmenu.add_command(label= "Cognitive Load Standard Deviation", command = self.__plot_cognitive_load_sd__)
            self.chartmenu.add_command(label= "Audio Response Time", command = self.__plot_audio_response__)
            self.chartmenu.add_command(label= "Heart Rate" , command = self.__plot_heart_rate__)
            self.chartmenu.add_command(label= "Heart Rate Variability", command = self.__plot_heart_rate_sd__)
            self.chartmenu.add_command(label= "Pupil Dilation" , command =self.__plot_pupil_dilation__)
            self.chartmenu.add_command(label= "Pupil Distance" , command = self.__plot_pupil_distance__)
            self.chartmenu.add_command(label= "Eye Openness", command = self.__plot_openness__)
            self.chartmenu.add_command(label= "Heart Rate x Cognitive Load" , command = self.__plot_hr_cl__)
            self.chartmenu.add_command(label= "Pupil Distance x Cognitive Load" , command = self.__plot_pds_cl__)
            self.chartmenu.add_command(label= "Pupil Dilation x Cognitive Load", command = self.__plot_pdl_cl__)
            self.chartmenu.add_command(label= "Heart Rate x Pupil Distance x Cognitive Load", command = self.__plot_hr_pd_cl__)


            self.menubar.add_cascade(label="Charts", menu=self.chartmenu, state='disabled' )

        # Enable the command "Charts"
        def __enablechartmenu__(self):
            self.menubar.entryconfig("Charts", state="normal")

        # Create a list of checkboxes and the variables and commands linked to each one
        def __createchekboxes__(self):
            self.MathCorrect = IntVar()
            self.MathWrong = IntVar()
            self.StabilizationCorrect = IntVar()
            self.StabilizationWrong = IntVar()
            self.AudioCorrect = IntVar()
            self.AudioWrong = IntVar()
            self.Tutorial = IntVar()
            self.Phase1 = IntVar()
            self.Pause = IntVar()
            self.Phase2 = IntVar()
            self.End = IntVar()

            self.MyCheckboxes = [Checkbutton(self.master, text='Math Correct',variable=self.MathCorrect, command=self.__plot_checked__),
                                 Checkbutton(self.master, text='Math Wrong',variable=self.MathWrong, command=self.__plot_checked__),
                                 Checkbutton(self.master, text='Audio Correct',variable=self.AudioCorrect, command=self.__plot_checked__),
                                 Checkbutton(self.master, text='Audio Wrong',variable=self.AudioWrong, command=self.__plot_checked__),
                                 Checkbutton(self.master, text='Stabilization Correct',variable=self.StabilizationCorrect, command=self.__plot_checked__),
                                 Checkbutton(self.master, text='Stabilization Wrong',variable=self.StabilizationWrong, command=self.__plot_checked__),
                                 Checkbutton(self.master, text='Tutorial',variable=self.Tutorial, command=self.__plot_checked__),
                                 Checkbutton(self.master, text='Phase1',variable=self.Phase1, command=self.__plot_checked__),
                                 Checkbutton(self.master, text='Phase2',variable=self.Phase2, command=self.__plot_checked__),
                                 Checkbutton(self.master, text='End',variable=self.End, command=self.__plot_checked__),
                                 Checkbutton(self.master, text='Pause',variable=self.Pause, command=self.__plot_checked__)]

        # Verify the scene which the csv was generated and pack the correct checkboxes
        def __enablecheckboxes__(self):
            if(self.Scene == "Calcul"):
                
                self.MyCheckboxes[6].pack(side=LEFT)
                self.MyCheckboxes[7].pack(side=LEFT)
                self.MyCheckboxes[8].pack(side=LEFT)
                self.MyCheckboxes[9].pack(side=LEFT)
                self.MyCheckboxes[10].pack(side=LEFT)
                self.MyCheckboxes[0].pack(side=LEFT)
                self.MyCheckboxes[1].pack(side=LEFT)


            elif(self.Scene == "DoubleTask"):
                
                self.MyCheckboxes[6].pack(side=LEFT)
                self.MyCheckboxes[7].pack(side=LEFT)
                self.MyCheckboxes[8].pack(side=LEFT)
                self.MyCheckboxes[9].pack(side=LEFT)
                self.MyCheckboxes[10].pack(side=LEFT)
                self.MyCheckboxes[0].pack(side=LEFT)
                self.MyCheckboxes[1].pack(side=LEFT)
                self.MyCheckboxes[4].pack(side=LEFT)
                self.MyCheckboxes[5].pack(side=LEFT)
            
            else:
                self.MyCheckboxes[0].pack(side=LEFT)
                self.MyCheckboxes[1].pack(side=LEFT)
                self.MyCheckboxes[2].pack(side=LEFT)
                self.MyCheckboxes[3].pack(side=LEFT)
                self.MyCheckboxes[4].pack(side=LEFT)
                self.MyCheckboxes[5].pack(side=LEFT)

        # Unpack all the checkboxes
        def __disablecheckboxes__(self):
            for item in self.MyCheckboxes:
                item.pack_forget()

        # Unpack all checkboxes, pack the correct checkboxes, and call __plot_cognitive_load__
        def __call_plot_cognitive_load__(self):
            self.__disablecheckboxes__()
            self.__enablecheckboxes__()
            self.__plot_cognitive_load__()

        # Read the csv file and 
        def __readfile__(self):

            # Open the "File Browser" and get the file path
            path = askopenfilename()

            # Read the csv data 
            data = pd.read_csv(path, delimiter=";")

            # Verify the scene by the columns in the csv file  
            if("Stabilization" in data):
                if("TaskAudio" in data):
                    self.Scene = "TripleTask"
                else:
                    self.Scene = "DoubleTask"
            else:
                self.Scene = "Calcul"

            # Manipulate data to plot the graphs
            self.data = self.__convert_data_type__(data)
            self.data['data'] = self.__pupil_distance__(self.data['data'])
            
            # Enable the chart menu after read the csv file
            self.__enablechartmenu__()
            
        # Plot Cognitive Load
        def __plot_cognitive_load__(self):
            self.chart = '__plot_cognitive_load__'
            self.__disablecheckboxes__()
            self.__enablecheckboxes__()
            # Configure the graph, figure and axis 
            data = self.data['data']
            self.fig.clear()
            self.plt = self.fig.add_subplot(111)
            line, = self.plt.plot(data["Time"],data["CognitiveLoadValue"]) 
            self.plt.set_ylim(0,1)
            self.plt.set_xlabel("Time")
            self.plt.set_ylabel("CognitiveLoad")
            self.plt.set_title("Cognitive Load x Time")
            self.labels = ["Cognitive Load"]
            self.handles = [line]
            
            # Formatting axis x with minutes and seconds
            formatter = matplotlib.ticker.FuncFormatter(lambda ms, x: time.strftime('%M:%S', time.gmtime(ms // 1000)))
            self.plt.xaxis.set_major_formatter(formatter)

            #Update the canvas
            self.canvas.draw()
            self.toolbar.update()

        # Plot Cognitive Load Standard Deviation
        def __plot_cognitive_load_sd__(self):
            self.chart = "__plot_cognitive_load_sd__"
            self.__disablecheckboxes__()
            self.__enablecheckboxes__()
            # Configure the graph, figure and axis 
            data = self.data['data']
            self.fig.clear()
            self.plt = self.fig.add_subplot(111)
            line, = self.plt.plot(data["Time"],data["CognitiveLoadStandardDeviation"])
            self.plt.set_xlabel("Time")
            self.plt.set_ylabel("Cognitive Load Standard Deviation")
            self.plt.set_title("Cognitive Load Standard Deviation x Time")

            self.labels = ["Cognitive Load"]
            self.handles = [line]

            # Formatting axis x with minutes and seconds
            formatter = matplotlib.ticker.FuncFormatter(lambda ms, x: time.strftime('%M:%S', time.gmtime(ms // 1000)))
            self.plt.xaxis.set_major_formatter(formatter)
            
            self.labels = ["Cognitive Load Sd"]
            self.handles = [line]

            # Update Canvas
            self.canvas.draw()
            self.toolbar.update()   
        
        # Plot Audio Response Time
        def __plot_audio_response__(self):
            
            # Unpack checkboxes
            self.__disablecheckboxes__()
            
            # Configure the graph, figure and axis 
            self.fig.clear()
            self.plt = self.fig.add_subplot(111)
            data = self.data['data']
            #self.plt.plot(data["Time"],data["ResponseTime"])
            self.plt.scatter(data["Time"],data["ResponseTime"],s=10,label='Right Eye Openness', marker='o')
            self.plt.set_xlabel("Time")
            self.plt.set_ylabel("Response Time")
            self.plt.set_title("Response Time x Time")

            # Formatting axis x with minutes and seconds
            formatter = matplotlib.ticker.FuncFormatter(lambda ms, x: time.strftime('%M:%S', time.gmtime(ms // 1000)))
            self.plt.xaxis.set_major_formatter(formatter)

            """ self.labels = ["Audio Response Time"]
            self.handles = [line]
            self.__plot_stages__(self.plt,self.labels,self.handles)
            self.plt.legend(self.handles = self.handles, self.labels = self.labels) """

            #Update Canvas
            self.canvas.draw()
            self.toolbar.update()

        # Plot Heart Rate
        def __plot_heart_rate__(self):
            self.chart = "__plot_heart_rate__"
            self.__disablecheckboxes__()
            self.__enablecheckboxes__()
            
            # Configure the graph, figure and axis 
            self.fig.clear()
            self.plt = self.fig.add_subplot(111)
            data = self.data['data']
            line, = self.plt.plot(data["Time"],data["HeartRate"]) 
            self.plt.set_xlabel("Time")
            self.plt.set_ylabel("HeartRate")
            self.plt.set_title("Heart Rate x Time")
            self.labels = ["Heart Rate"]
            self.handles = [line]
            # Formatting axis x with minutes and seconds
            formatter = matplotlib.ticker.FuncFormatter(lambda ms, x: time.strftime('%M:%S', time.gmtime(ms // 1000)))
            self.plt.xaxis.set_major_formatter(formatter)

            #Update Canvas
            self.canvas.draw()
            self.toolbar.update()

        # Plot Heart Rate Standard Deviation
        def __plot_heart_rate_sd__(self):
            self.chart = "__plot_heart_rate_sd__"
            self.__disablecheckboxes__()
            self.__enablecheckboxes__()

            # Configure the graph, figure and axis 
            self.fig.clear()
            self.plt = self.fig.add_subplot(111)
            data = self.data['data']
            line, = self.plt.plot(data["Time"],data["HeartRateVariabilitySdnn"]) 
            self.plt.set_xlabel("Time")
            self.plt.set_ylabel("HeartRateVariability")
            self.plt.set_title("Heart Rate Variability x Time")
            self.labels = ["Heart Rate Sd"]
            self.handles = [line]
            # Formatting axis x with minutes and seconds
            formatter = matplotlib.ticker.FuncFormatter(lambda ms, x: time.strftime('%M:%S', time.gmtime(ms // 1000)))
            self.plt.xaxis.set_major_formatter(formatter)

            #Update Canvas
            self.canvas.draw()
            self.toolbar.update()

        #Plot Heart Rate x Cognitive Load
        def __plot_hr_cl__(self):
            self.__disablecheckboxes__()
            # Unpack checkboxes
            
            # Configure figure
            self.fig.clear()
            data = self.data['data']
            self.plt = self.fig.add_subplot(111)

            # Create a duplicate of the figure
            cl = self.plt.twinx()
            
            # Plot in the first figure Heart Rate x Time
            p1, = self.plt.plot(data["Time"],data["HeartRate"],"tab:blue",label='Heart Rate') 

            # Plot in the second figure CognitiveLoadValue Rate x Time
            p2, = cl.plot(data["Time"],data["CognitiveLoadValue"],"tab:red", label= 'Cognitive Load')
            
            # Set self.labels and colors of each figure
            self.plt.set_xlabel("Time")
            self.plt.set_ylabel("Heart Rate")
            cl.set_ylabel("Cognitive Load")
            self.plt.yaxis.label.set_color(p1.get_color())
            cl.yaxis.label.set_color(p2.get_color())
            tkw = dict(size=4, width=1.5)
            self.plt.tick_params(axis='y', colors=p1.get_color(), **tkw)
            cl.tick_params(axis='y', colors=p2.get_color(), **tkw)
            self.plt.tick_params(axis='x', **tkw)
            self.plt.legend(handles=[p1, p2])
            self.plt.set_title("Heart Rate x Cognitive Load")
            # Formatting axis x with minutes and seconds
            formatter = matplotlib.ticker.FuncFormatter(lambda ms, x: time.strftime('%M:%S', time.gmtime(ms // 1000)))
            self.plt.xaxis.set_major_formatter(formatter)

            # Update Canvas
            self.canvas.draw() 
            self.toolbar.update()

        #Plot Pupil Dilation
        def __plot_pupil_dilation__(self):
            self.__disablecheckboxes__()

            # Configure the graph, figure and axis 
            self.fig.clear()
            self.plt = self.fig.add_subplot(111)
            data = self.data['data']
            line1, = self.plt.plot(data["Time"],data["RightEyePupilDilation"],"tab:blue",label='Right Eye Pupil Dilation')
            line2, = self.plt.plot(data["Time"],data["LeftEyePupilDilation"],"tab:red",label='Left Eye Pupil Dilation')
            self.plt.set_xlabel("Time")
            self.plt.set_ylabel("Pupil Dilation")
            self.plt.legend()
            self.plt.set_title("Pupil Dilation x Time")
            # Formatting axis x with minutes and seconds
            formatter = matplotlib.ticker.FuncFormatter(lambda ms, x: time.strftime('%M:%S', time.gmtime(ms // 1000)))
            self.plt.xaxis.set_major_formatter(formatter)

            #Update Canvas
            self.canvas.draw()
            self.toolbar.update()

        #Plot Eye Openness
        def __plot_openness__(self):
            
            # Unpack checkboxes
            self.__disablecheckboxes__()

            # Configure the graph, figure and axis 
            self.fig.clear()
            self.plt = self.fig.add_subplot(111)
            data = self.data['data']
            self.plt.scatter(data["Timestamp"],data["RightEyeOpenness"],s=10,label='Right Eye Openness', marker='o')
            self.plt.scatter(data["Timestamp"],data["LeftEyeOpenness"],s=10, label='Left Eye Openness', marker='o')
            self.plt.set_xlabel("Time")
            self.plt.set_ylabel("Eye Openness")
            self.plt.legend()
            self.plt.set_title("Eye Openness x Time")
            
            # Formatting axis x with minutes and seconds
            formatter = matplotlib.ticker.FuncFormatter(lambda ms, x: time.strftime('%M:%S', time.gmtime(ms // 1000)))
            self.plt.xaxis.set_major_formatter(formatter)

            #Update Canvas
            self.canvas.draw()
            self.toolbar.update()

        #Plot Pupil Distance
        def __plot_pupil_distance__(self):
            # Unpack checkboxes
            self.__disablecheckboxes__()
            
            # Configure the graph, figure and axis 
            self.fig.clear()
            self.plt = self.fig.add_subplot(111)
            data = self.data['data']
            self.plt.plot(data["Time"],data["RightEyePupilDistance"],"tab:blue",label='Right Eye Pupil Distance')
            self.plt.plot(data["Time"],data["LeftEyePupilDistance"],"tab:red",label='Left Eye Pupil Distance')
            self.plt.set_xlabel("Time")
            self.plt.set_ylabel("Pupil Distance")
            self.plt.legend()
            self.plt.set_title("Pupil Distance x Time")
            # Formatting axis x with minutes and seconds
            formatter = matplotlib.ticker.FuncFormatter(lambda ms, x: time.strftime('%M:%S', time.gmtime(ms // 1000)))
            self.plt.xaxis.set_major_formatter(formatter)

            #Update Canvas
            self.canvas.draw()
            self.toolbar.update()
        
        #Plot Pupil Distance x Cognitive Load
        def __plot_pds_cl__(self):
            # Unpack checkboxes
            self.__disablecheckboxes__()
            
            # Configure figure
            self.fig.clear()
            self.plt = self.fig.add_subplot(111)

            data = self.data['data']
            
            # Create a duplicate of the figure
            cl = self.plt.twinx()
            
            # Plot in the first figure Right Eye Pupil Distance x Time
            p1, = self.plt.plot(data["Time"],data["RightEyePupilDistance"],"tab:blue",label='Right Eye Pupil Distance') 
            p2, = self.plt.plot(data["Time"],data["LeftEyePupilDistance"],"tab:green",label='Left Eye Pupil Distance') 

            # Plot in the second figure Right Cognitive Load x Time
            p3, = cl.plot(data["Time"],data["CognitiveLoadValue"],"tab:red", label= 'Cognitive Load')

            # Set self.labels and colors of each figure
            self.plt.set_xlabel("Time")
            self.plt.set_ylabel("Pupil Distance")
            cl.set_ylabel("Cognitive Load")
            cl.yaxis.label.set_color(p3.get_color())
            
            tkw = dict(size=4, width=1.5)
            cl.tick_params(axis='y', colors=p3.get_color(), **tkw)
            self.plt.tick_params(axis='x', **tkw)
            self.plt.legend(handles=[p1, p2, p3])
            self.plt.set_title("Pupil Distance x Cognitive Load")
            
            # Formatting axis x with minutes and seconds
            formatter = matplotlib.ticker.FuncFormatter(lambda ms, x: time.strftime('%M:%S', time.gmtime(ms // 1000)))
            self.plt.xaxis.set_major_formatter(formatter)

            # Update Canvas
            self.canvas.draw()
            self.toolbar.update()

        #Ploting Pupil Dilation x Cognitive Load
        def __plot_pdl_cl__(self):
            # Unpack checkboxes
            self.__disablecheckboxes__()
            
            # Configure figure
            self.fig.clear()
            self.plt = self.fig.add_subplot(111)

            data = self.data['data']

            # Create a duplicate of the figure
            cl = self.plt.twinx()
            
            # Plot in the first figure Right Eye Pupil Dilation x Time
            p1, = self.plt.plot(data["Time"],data["RightEyePupilDilation"],"tab:blue",label='Right Eye Pupil Dilation') 
            p2, = self.plt.plot(data["Time"],data["LeftEyePupilDilation"],"tab:green",label='Left Eye Pupil Dilation') 

            # Plot in the second figure Right Cognitive Load x Time
            p3, = cl.plot(data["Time"],data["CognitiveLoadValue"],"tab:red", label= 'Cognitive Load')

            # Set self.labels and colors of each figure
            self.plt.set_xlabel("Time")
            self.plt.set_ylabel("Pupil Dilation")
            cl.set_ylabel("Cognitive Load")
            cl.yaxis.label.set_color(p3.get_color())
            
            tkw = dict(size=4, width=1.5)
            cl.tick_params(axis='y', colors=p3.get_color(), **tkw)
            self.plt.tick_params(axis='x', **tkw)
            self.plt.legend(handles=[p1, p2, p3])
            self.plt.set_title("Pupil Dilation x Cognitive Load")

            # Formatting axis x with minutes and seconds
            formatter = matplotlib.ticker.FuncFormatter(lambda ms, x: time.strftime('%M:%S', time.gmtime(ms // 1000)))
            self.plt.xaxis.set_major_formatter(formatter)

            # Update Canvas
            self.canvas.draw()
            self.toolbar.update()

        #Plot Pupil Distance x Heart Rate x Cognitive Load
        def __plot_hr_pd_cl__(self):
            # Unpack checkboxes
            self.__disablecheckboxes__()

            # Configure figure
            self.fig.clear()
            self.plt = self.fig.add_subplot(111)

            data = self.data['data']
            
            # Create two duplicate of the figure
            cl = self.plt.twinx()
            hr = self.plt.twinx()

            # Change the axis position of the third figure
            hr.spines.right.set_position(("axes", 0))

            # Plot in the first figure Eye Pupil Distance x Time
            p1, = self.plt.plot(data["Time"],data["RightEyePupilDistance"],"tab:blue",label='Right Eye Pupil Distance') 
            p2, = self.plt.plot(data["Time"],data["LeftEyePupilDistance"],"tab:green",label='Left Eye Pupil Distance') 

            # Plot in the second figure Cognitive Load x Time
            p3, = cl.plot(data["Time"],data["CognitiveLoadValue"],"tab:red", label= 'Cognitive Load')

            # Plot in the third figure Heart Rate x Time
            p4, = hr.plot(data["Time"],data["HeartRate"],"tab:orange", label= 'Heart Rate')

            # Set self.labels and colors of each figure
            self.plt.set_xlabel("Time")
            self.plt.set_ylabel("Pupil Distance")
            cl.set_ylabel("Cognitive Load")
            hr.set_ylabel("HeartRate")

            cl.yaxis.label.set_color(p3.get_color())
            hr.yaxis.label.set_color(p4.get_color())
            
            tkw = dict(size=4, width=1.5)
            cl.tick_params(axis='y', colors=p3.get_color(), **tkw)
            hr.tick_params(axis='y', colors=p4.get_color(), **tkw)
            self.plt.tick_params(axis='x', **tkw)
            self.plt.legend(handles=[p1, p2, p3, p4])
            self.plt.set_title("Pupil Distance x Heart Rate x Cognitive Load")

            # Formatting axis x with minutes and seconds
            formatter = matplotlib.ticker.FuncFormatter(lambda ms, x: time.strftime('%M:%S', time.gmtime(ms // 1000)))
            self.plt.xaxis.set_major_formatter(formatter)

            # Update Canvas
            self.canvas.draw()
            self.toolbar.update()

        def __plot_checked__(self):
            
            if(self.chart == '__plot_cognitive_load__'):
                    self.__plot_cognitive_load__()
            if(self.chart == '__plot_heart_rate_sd__'):
                    self.__plot_heart_rate_sd__()
            if(self.chart == '__plot_heart_rate__'):
                    self.__plot_heart_rate__()
            if(self.chart == '__plot_cognitive_load_sd__'):
                    self.__plot_cognitive_load_sd__()


            data = self.data['data']
            myplot = self.plt
            myhandles = self.handles
            mylabels = self.labels

            if self.Tutorial.get():
                # Tutoriel
                if(len(self.data['Tutoriel']) != 0):
                    x = data.loc[self.data['Tutoriel'][0],'Time']        
                    tutoriel = myplot.axvline(x=x, label='', c='slategray')
                    mylabels.append('Tutoriel')
                    myhandles.append(tutoriel)
            
            if self.Phase1.get():
                # Phase 1
                #ymax = data.loc[self.data['AudioCorrect'][i], 'CognitiveLoadValue']
                if(len(self.data['Phase1'])  != 0):
                    x = data.loc[self.data['Phase1'][0],'Time']        
                    phase1 = myplot.axvline(x=x, label='', c='slategray')
                    mylabels.append('Phase 1')
                    myhandles.append(phase1)

            if self.Pause.get():
                # Pause
                if(len(self.data['Pause'])  != 0):
                    x = data.loc[self.data['Pause'][0],'Time']        
                    pause = myplot.axvline(x=x, label='', c='slategray')
                    mylabels.append('Pause')
                    myhandles.append(pause)

            if self.Phase2.get():
                # Phase 2
                if(len(self.data['Phase2'])  != 0):
                    x = data.loc[self.data['Phase2'][0],'Time']        
                    phase2 = myplot.axvline(x=x, label='', c='slategray')
                    mylabels.append('Phase 2')
                    myhandles.append(phase2)
            
            if self.End.get():
                # End
                if(len(self.data['End'])  != 0):
                    x = data.loc[self.data['End'][0],'Time']     
                    end = myplot.axvline(x=x, label='', c='slategray')
                    mylabels.append('End')
                    myhandles.append(end)
            
            
            # Verifying if the variable AudioCorrect is true (chekbox checked) and trace a line at the correct moment
            if self.AudioCorrect.get():
                for i in range(len(self.data['AudioCorrect'])):
                    #ymax = data.loc[self.data['AudioCorrect'][i], 'CognitiveLoadValue']
                    x = data.loc[self.data['AudioCorrect'][i],'Time']
                    """ if(str(ymax) == "nan"):
                        line_audio_correct = myplot.axvline(x=x ,label='', c='gold')
                    else: """
                    line_audio_correct = myplot.axvline(x=x, label='', c='gold')

                mylabels.append('Audio Answer Correct')
                myhandles.append(line_audio_correct)

            # Verifying if the variable AudioWrong is true (chekbox checked) and trace a line at the correct moment
            if self.AudioWrong.get():
                for i in range(len(self.data['AudioWrong'])):
                    #ymax = data.loc[self.data['AudioWrong'][i], 'CognitiveLoadValue']
                    x = data.loc[self.data['AudioWrong'][i],'Time']
                    """ if(str(ymax) == "nan"):
                        line_audio_wrong = myplot.axvline(x=x ,label='', c='sienna')
                    else: """
                    line_audio_wrong =  myplot.axvline(x=x, label='', c='sienna')
                
                mylabels.append('Audio Answer Wrong')
                myhandles.append(line_audio_wrong)


            # Verifying if the variable MathCorrect is true (chekbox checked) and trace a line at the correct moment
            if self.MathCorrect.get():
                for i in range(len(self.data['MathCorrect'])):
                    #ymax = data.loc[self.data['MathCorrect'][i], 'CognitiveLoadValue']
                    x = data.loc[self.data['MathCorrect'][i],'Time']
                    """ if(str(ymax) == "nan"):
                        line_math_correct = myplot.axvline(x=x ,label='', c='lawngreen')
                    else: """
                    line_math_correct = myplot.axvline(x=x, label='', c='lawngreen')
                mylabels.append('Math Answer Correct')
                myhandles.append(line_math_correct)
                
            # Verifying if the variable MathWrong is true (chekbox checked) and trace a line at the correct moment
            if self.MathWrong.get():
                for i in range(len(self.data['MathWrong'])):
                    #ymax = data.loc[self.data['MathWrong'][i], 'CognitiveLoadValue']
                    x = data.loc[self.data['MathWrong'][i],'Time']
                    """ if(str(ymax) == "nan"):
                        line_math_wrong = myplot.axvline(x=x ,label='', c='red')
                    else: """
                    line_math_wrong = myplot.axvline(x=x,  label='', c='red')
                mylabels.append('Math Answer Wrong')
                myhandles.append(line_math_wrong)
            
            # Verifying if the variable StabilizationCorrect is true (chekbox checked) and trace a line at the correct moment
            if self.StabilizationCorrect.get():
                for i in range(len(self.data['StabilizationCorrect'])):
                    #ymax = data.loc[self.data['StabilizationCorrect'][i], 'CognitiveLoadValue']
                    x = data.loc[self.data['StabilizationCorrect'][i],'Time']
                    """ if(str(ymax) == "nan"):
                        line_stabilization_correct = myplot.axvline(x=x ,label='', c='darkorange')
                    else: """
                    line_stabilization_correct = myplot.axvline(x=x,  label='', c='darkorange')
                mylabels.append('Stabilization Correct')
                myhandles.append(line_stabilization_correct)
                
            # Verifying if the variable StabilizationWrong is true (chekbox checked) and trace a line at the correct moment
            if self.StabilizationWrong.get():
                for i in range(len(self.data['StabilizationWrong'])):
                    #ymax = data.loc[self.data['StabilizationWrong'][i], 'CognitiveLoadValue']
                    x = data.loc[self.data['StabilizationWrong'][i],'Time']
                    """ if(str(ymax) == "nan"):
                        line_stabilization_wrong = myplot.axvline(x=x ,label='', c='magenta')
                    else: """
                    line_stabilization_wrong = myplot.axvline(x=x,  label='', c='magenta')
                
                mylabels.append('Stabilization Wrong')
                myhandles.append(line_stabilization_wrong)

            self.plt.legend(handles = myhandles, labels = mylabels)
            self.canvas.draw()
            self.toolbar.update()

        #Convert Data to plot
        def __convert_data_type__(self,data):
            
            # Get Date Time from Timestamp
            data["Timestamp"] = pd.to_datetime(data["Timestamp"], unit = 'ms')

            for i in range(len(data.index)):
                if(i != 0):
                    data.loc[i,'Time'] = data.loc[i,'Timestamp'] - data.loc[0,'Timestamp']
                    data.loc[i,"Time"] =  round(data.loc[i,'Time'].total_seconds()*1000)

            data.loc[0,'Time'] = data.loc[0,'Timestamp'] - data.loc[0,'Timestamp']
            data.loc[0,"Time"] =  round(data.loc[0,'Time'].total_seconds()*1000)

            # Replace all ',' by '.' to transform to float
            if(data.dtypes["CognitiveLoadValue"] == object):
                data["CognitiveLoadValue"] = data["CognitiveLoadValue"].str.replace(',','.')

            if(data.dtypes["CognitiveLoadStandardDeviation"] == object):
                data["CognitiveLoadStandardDeviation"] = data["CognitiveLoadStandardDeviation"].str.replace(',','.')

            if(data.dtypes["ResponseTime"] == object):
                data["ResponseTime"] = data["ResponseTime"].str.replace(',','.')
                data["ResponseTime"] = data["ResponseTime"].replace({' ':None})
                data["ResponseTime"] = pd.to_numeric(data["ResponseTime"], downcast='float')


            if(data.dtypes["HeartRateVariabilitySdnn"] == object):
                data["HeartRateVariabilitySdnn"] = data["HeartRateVariabilitySdnn"].str.replace(',','.')    
            
            if(data.dtypes["RightEyePupilPositionX"] == object):
                data["RightEyePupilPositionX"] = data["RightEyePupilPositionX"].str.replace(',','.')

            if(data.dtypes["RightEyePupilPositionY"] == object):
                data["RightEyePupilPositionY"] = data["RightEyePupilPositionY"].str.replace(',','.')

            if(data.dtypes["LeftEyePupilPositionX"] == object):
                data["LeftEyePupilPositionX"] = data["LeftEyePupilPositionX"].str.replace(',','.')

            if(data.dtypes["LeftEyePupilPositionY"] == object):
                data["LeftEyePupilPositionY"] = data["LeftEyePupilPositionY"].str.replace(',','.')

            if(data.dtypes["LeftEyePupilDilation"] == object):
                data["LeftEyePupilDilation"] = data["LeftEyePupilDilation"].str.replace(',','.')
                data["LeftEyePupilDilation"] = data["LeftEyePupilDilation"].replace({'-1':None})
                data["LeftEyePupilDilation"] = pd.to_numeric(data["LeftEyePupilDilation"], downcast='float')

            if(data.dtypes["RightEyePupilDilation"] == object):
                data["RightEyePupilDilation"] = data["RightEyePupilDilation"].str.replace(',','.')
                data["RightEyePupilDilation"] = data["RightEyePupilDilation"].replace({'-1':None})
                data["RightEyePupilDilation"] = pd.to_numeric(data["RightEyePupilDilation"], downcast='float')
                

            # Verify the scene that generated the csv file to read the task answer columns
            if(self.Scene == "TripleTask"):

                # DeEndes a list of each task answer at the csv, separating into correct and wrong answers 
                AudioStart = []
                AudioEnd = []
                AudioWrong = []
                AudioCorrect = []

                MathStart = []
                MathEnd = []
                MathWrong = []
                MathCorrect = []

                StabilizationWrong = []
                StabilizationCorrect = []
                
                

                # For each event wrote in the csv append to the respective list
                for i in range(len(data.index)):
                    if(data.loc[i, 'TaskAudio'] == 'Start'):
                        AudioStart.append(i)

                    if(data.loc[i, 'TaskAudio'] == 'End'):
                        AudioEnd.append(i)

                    if(data.loc[i, 'AnswerAudio'] == 'Correct'):
                        AudioCorrect.append(i)

                    if(data.loc[i, 'AnswerAudio'] == 'Wrong'):
                        AudioWrong.append(i)

                    if(data.loc[i, 'TaskMath'] == 'Start'):
                        MathStart.append(i)

                    if(data.loc[i, 'TaskMath'] == 'End'):
                        MathEnd.append(i)

                    if(data.loc[i, 'AnswerMath'] == 'Correct'):
                        MathCorrect.append(i)

                    if(data.loc[i, 'AnswerMath'] == 'Wrong'):
                        MathWrong.append(i)

                    if(data.loc[i, 'Stabilization'] == 'Wrong Direction'):
                        StabilizationWrong.append(i)

                    if(data.loc[i, 'Stabilization'] == 'Correct Direction'):
                        StabilizationCorrect.append(i)

                    
                # Add fields to the dataframe corresponding the list created
                dataDict = dict();
                dataDict['data'] = data
                dataDict['AudioStart'] = AudioStart
                dataDict['AudioEnd'] = AudioEnd
                dataDict['AudioCorrect'] = AudioCorrect
                dataDict['AudioWrong'] = AudioWrong

                dataDict['MathStart'] = MathStart
                dataDict['MathEnd'] = MathEnd
                dataDict['MathCorrect'] = MathCorrect
                dataDict['MathWrong'] = MathWrong

                dataDict['StabilizationCorrect'] = StabilizationCorrect
                dataDict['StabilizationWrong'] = StabilizationWrong
            
            #DataFrame Calcul
            elif(self.Scene == "Calcul"):

                # DeEndes a list of each task answer at the csv, separating into correct and wrong answers 
                MathWrong = []
                MathCorrect = []

                # For each event wrote in the csv append to the respective list
                for i in range(len(data.index)):
                    if(data.loc[i, 'AnswerMath'] == 'Correct'):
                            MathCorrect.append(i)

                    if(data.loc[i, 'AnswerMath'] == 'Wrong'):
                        MathWrong.append(i)
                
                # Add fields to the dataframe corresponding the list created
                dataDict = dict();
                dataDict['data'] = data
                dataDict['MathCorrect'] = MathCorrect
                dataDict['MathWrong'] = MathWrong

            else:

                # Defines a list of each task answer at the csv, separating into correct and wrong answers 
                MathWrong = []
                MathCorrect = []
                StabilizationWrong = []
                StabilizationCorrect = []

                # For each event wrote in the csv append to the respective list
                for i in range(len(data.index)):
                    if(data.loc[i, 'AnswerMath'] == 'Correct'):
                            MathCorrect.append(i)

                    if(data.loc[i, 'AnswerMath'] == 'Wrong'):
                        MathWrong.append(i)

                    if(data.loc[i, 'Stabilization'] == 'Wrong'):
                        StabilizationWrong.append(i)

                    if(data.loc[i, 'Stabilization'] == 'Correct'):
                        StabilizationCorrect.append(i)
                
                # Add fields to the dataframe corresponding the list created
                dataDict = dict();
                dataDict['data'] = data
                dataDict['MathCorrect'] = MathCorrect
                dataDict['MathWrong'] = MathWrong
                dataDict['StabilizationCorrect'] = StabilizationCorrect
                dataDict['StabilizationWrong'] = StabilizationWrong
        

            # Create lists to get the start of each phase

            Tutoriel = []
            Phase1 = []
            Pause = []
            Phase2 = []
            End = []

            for i in range(len(data.index)):            
                if(data.loc[i, 'Stage'] == 'Tutoriel' ):
                    Tutoriel.append(i)
                    break
            
            for i in range(len(data.index)):            
                if(data.loc[i, 'Stage'] == 'Partie 1' ):
                    Phase1.append(i)
                    break
            
            for i in range(len(data.index)):            
                if(data.loc[i, 'Stage'] == 'Pause' ):
                    Pause.append(i)
                    break

            for i in range(len(data.index)):            
                if(data.loc[i, 'Stage'] == 'Partie 2' ):
                    Phase2.append(i)
                    break

            for i in range(len(data.index)):            
                if(data.loc[i, 'Stage'] == 'FIN' ):
                    End.append(i)
                    break

            dataDict['Tutoriel'] = Tutoriel
            dataDict['Phase1'] = Phase1
            dataDict['Pause'] = Pause
            dataDict['Phase2'] = Phase2
            dataDict['End'] = End
            
            return dataDict

        #Fill Pupil Distance Fields
        def __pupil_distance__(self,data):

            # Create a dataframe field Eye Pupil Distance for each eye
            data['RightEyePupilDistance'] = 0
            data['LeftEyePupilDistance'] = 0
            
            # Fill eache field of Eye Pupil Distance calculating the distance between a point and its immediately previous point
            for i in range(len(data.index)):
                if(i != 0):
                    data.loc[i,'RightEyePupilDistance'] = self.__calculate_distance__(float(data.loc[i,'RightEyePupilPositionX']),float(data.loc[i,'RightEyePupilPositionY']),float(data.loc[i-1,'RightEyePupilPositionX']),float(data.loc[i-1,'RightEyePupilPositionY']))   
                    data.loc[i,'LeftEyePupilDistance'] = self.__calculate_distance__(float(data.loc[i,'LeftEyePupilPositionX']),float(data.loc[i,'LeftEyePupilPositionY']),float(data.loc[i-1,'LeftEyePupilPositionX']),float(data.loc[i-1,'LeftEyePupilPositionY']))   
            return data

        # Return the distance between the two points passed
        def __calculate_distance__(self,xa,ya, xb, yb):
            if(xa == -1 or ya == -1 or xb == -1 or yb == -1):
                return None
            return math.sqrt(math.pow((xb - xa),2) + math.pow((yb - ya),2))


# Create the application class and run
root = Tk()
app = MainWindow(root)
root.mainloop()