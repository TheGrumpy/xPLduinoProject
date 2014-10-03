
EXTRA_DIST =  Makefile

# Warning: This is an automatically generated file, do not edit!

srcdir=.
top_srcdir=..

include $(top_srcdir)/config.make

ifeq ($(CONFIG),DEBUG_X86)
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG;" "-main:xPLduinoManager.MainClass"
ASSEMBLY = bin/Debug/xPLduino-Manager.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = winexe
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug

XPLDUINO_MANAGER_EXE_MDB_SOURCE=bin/Debug/xPLduino-Manager.exe.mdb
XPLDUINO_MANAGER_EXE_MDB=$(BUILD_DIR)/xPLduino-Manager.exe.mdb
PARAM_XML_SOURCE=Xml/Param.xml

endif

ifeq ($(CONFIG),RELEASE_X86)
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- "-main:xPLduinoManager.MainClass"
ASSEMBLY = bin/Release/xPLduino-Manager.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = winexe
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release

XPLDUINO_MANAGER_EXE_MDB=
PARAM_XML_SOURCE=Xml/Param.xml

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(XPLDUINO_MANAGER_EXE_MDB)  

PROGRAMFILES_XML = \
	$(PARAM_XML)  

BINARIES = \
	$(XPLDUINO_MANAGER)  


RESGEN=resgen2

PARAM_XML = $(BUILD_DIR)/Xml/Param.xml
XPLDUINO_MANAGER = $(BUILD_DIR)/xplduino-manager

FILES =  \
	AssemblyInfo.cs \
	Class/Board.cs \
	Class/DataManagement.cs \
	Class/Debug.cs \
	Class/Instance.cs \
	Class/Network.cs \
	Class/Node.cs \
	Class/Notebook.cs \
	Class/OutputTreeView.cs \
	Class/Param.cs \
	Class/Parameters.cs \
	Class/Pin.cs \
	Class/Project.cs \
	gtk-gui/generated.cs \
	gtk-gui/xPLduinoManager.BoardI2CProperties.cs \
	gtk-gui/xPLduinoManager.I2CProperties.cs \
	gtk-gui/xPLduinoManager.InstanceProperties.cs \
	gtk-gui/xPLduinoManager.MainWindow.cs \
	gtk-gui/xPLduinoManager.NewBoard.cs \
	gtk-gui/xPLduinoManager.NewInstance.cs \
	gtk-gui/xPLduinoManager.NewNetwork.cs \
	gtk-gui/xPLduinoManager.NewNode.cs \
	gtk-gui/xPLduinoManager.NewProject.cs \
	gtk-gui/xPLduinoManager.NodeProperties.cs \
	gtk-gui/xPLduinoManager.OneWireProperties.cs \
	gtk-gui/xPLduinoManager.ProjectProperties.cs \
	gtk-gui/xPLduinoManager.test.cs \
	gtk-gui/xPLduinoManager.Welcome.cs \
	Main.cs \
	MainWindow.cs \
	Param/Boards.cs \
	Param/Instances.cs \
	Param/Networks.cs \
	Widgets/BoardI2CProperties.cs \
	Widgets/I2CProperties.cs \
	Widgets/InstanceProperties.cs \
	Widgets/NodeProperties.cs \
	Widgets/OneWireProperties.cs \
	Widgets/ProjectProperties.cs \
	Widgets/test.cs \
	Widgets/Welcome.cs \
	Windows/NewBoard.cs \
	Windows/NewInstance.cs \
	Windows/NewNetwork.cs \
	Windows/NewNode.cs \
	Windows/NewProject.cs 

DATA_FILES = 

RESOURCES =  \
	bin/Debug/Images/xplduino-FirstPage.png,xPLduinoManager.bin.Debug.Images.xplduino-FirstPage.png \
	gtk-gui/gui.stetic 

EXTRAS = \
	app.desktop \
	Xml/Param.xml \
	Class \
	Xml \
	Windows \
	Widgets \
	Param \
	xplduino-manager.in 

REFERENCES =  \
	Mono.Cairo \
	Mono.Posix \
	-pkg:glade-sharp-2.0 \
	-pkg:glib-sharp-2.0 \
	-pkg:gtk-sharp-2.0 \
	System \
	System.Windows.Forms \
	System.Xml \
	System.Xml.Linq

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(PROGRAMFILES_XML) $(BINARIES) 

#Targets
all: $(ASSEMBLY) $(PROGRAMFILES) $(PROGRAMFILES_XML) $(BINARIES)  $(top_srcdir)/config.make

include $(top_srcdir)/Makefile.include
#include $(srcdir)/custom-hooks.make



$(eval $(call emit-deploy-target,PARAM_XML))
$(eval $(call emit-deploy-wrapper,XPLDUINO_MANAGER,xplduino-manager,x))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'


$(ASSEMBLY_MDB): $(ASSEMBLY)
$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	make pre-all-local-hook prefix=$(prefix)
	mkdir -p $(shell dirname $(ASSEMBLY))
	make $(CONFIG)_BeforeBuild
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
	make $(CONFIG)_AfterBuild
	make post-all-local-hook prefix=$(prefix)

install-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-install-local-hook prefix=$(prefix)
	make install-satellite-assemblies prefix=$(prefix)
	mkdir -p '$(DESTDIR)$(libdir)/$(PACKAGE)'
	$(call cp,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(XPLDUINO_MANAGER_EXE_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	mkdir -p '$(DESTDIR)$(libdir)/$(PACKAGE)/Xml'
	$(call cp,$(PARAM_XML),$(DESTDIR)$(libdir)/$(PACKAGE)/Xml)
	mkdir -p '$(DESTDIR)$(bindir)'
	$(call cp,$(XPLDUINO_MANAGER),$(DESTDIR)$(bindir))
	make post-install-local-hook prefix=$(prefix)

uninstall-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-uninstall-local-hook prefix=$(prefix)
	make uninstall-satellite-assemblies prefix=$(prefix)
	$(call rm,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(XPLDUINO_MANAGER_EXE_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(PARAM_XML),$(DESTDIR)$(libdir)/$(PACKAGE)/Xml)
	$(call rm,$(XPLDUINO_MANAGER),$(DESTDIR)$(bindir))
	make post-uninstall-local-hook prefix=$(prefix)

install: install-local
uninstall: uninstall-local
clean: clean-local

include $(top_srcdir)/rules.make
