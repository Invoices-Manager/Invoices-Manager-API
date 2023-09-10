#use the microsoft sdk 6 image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

#set the work dir
WORKDIR /app

#copy all files into the workdir
COPY . .

#restore the dependencies and build the application
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

#create an image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

#set the work dir
WORKDIR /app

#copy the published application from the build container
COPY --from=build /app/publish .

#copy the needed file for the dotnet ef tool
COPY . .

#install wget to install the sdk6
RUN apt update
RUN apt install -y wget

#download and configure sdk-6
RUN wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb

#install dotnet sdk-6
RUN apt-get update
RUN apt-get install -y dotnet-sdk-6.0

#install the dotnet ef tool (entity framework)
RUN dotnet tool install --global dotnet-ef

#set the directory is in your PATH environment variable (to make the tool work)
RUN echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.bashrc

#start the api
CMD ["./Invoices-Manager-API"]