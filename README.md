
"# whatsapp messenger dotnet core 7"
<br/>
<br/>
<br/>
<b>prerequisites</b>
<hr> 
<ul>
  <li> dotnet core 7</li>
  <li> singnalr </li>
  <li> mongodb </li>
</ul>

<br/>
<br/>
<br/>
<h2>  How to use ? </h2>
<hr> 
<ul> 
  <li> /Account/Create => Register new user  </li>
  <li> /AddRole => Add role from all user   </li>
  <li> /Account/NewUser => Create random User</li>
  <li>/Account/ChangePassword =>  Change Password via Phone Number</li>
</ul>
<br/>
<h2> learn inistallation in windows </h2>
<p> <a href="https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-7.0.404-windows-x64-installer"> download dotnet core 7 sdk </a> </p>
 <p> <a href="https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-7.0.14-windows-x64-installer"> download dotnet core 7 runtime </a> </p>
 <p> <a href="https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-7.0.14-windows-hosting-bundle-installer">  serv in server : hosting bundle </a> </p>
  <p> <a href="https://www.mongodb.com/docs/manual/tutorial/install-mongodb-on-windows/">   download mongodb </a> </p>
  <br>
  <hr>
<h2> learn inistallation in  linux ubuntu  </h2>
<pre> sudo apt update -y</pre>
<pre>   wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb </pre>
<pre>   sudo dpkg -i packages-microsoft-prod.deb</pre>
<pre>  sudo apt update -y  </pre>
<pre> sudo apt install dotnet-sdk-7.0</pre>
<pre>  sudo apt update -y  </pre>
<pre> sudo apt install dotnet-sdk-7.0</pre>
<hr> 
<h3> install mongodb  </h3>
 <pre> sudo apt update
sudo apt dist-upgrade -y</pre>
<pre> sudo apt install gnupg</pre> 
<per> echo "deb http://security.ubuntu.com/ubuntu impish-security main" | sudo tee /etc/apt/sources.list.d/impish-security.list
sudo apt update
sudo apt install libssl1.1</per>
<pre> wget -qO - https://www.mongodb.org/static/pgp/server-5.0.asc | sudo apt-key add -
</pre>
<pre> 
  echo "deb [ arch=amd64,arm64 ] https://repo.mongodb.org/apt/ubuntu focal/mongodb-org/5.0 multiverse" | sudo tee /etc/apt/sources.list.d/mongodb-org-5.0.list

</pre>

<pre> sudo apt update
sudo apt install -y mongodb-org</pre>
<pre>
  sudo systemctl enable mongod
</pre>
<pre>
  sudo service mongod start
</pre>
<pre> 
  sudo service mongod status
</pre>

<pre> 
sudo nano /etc/mongod.conf
</pre>
<pre>
  sudo systemctl enable mongodb
<pre> 
sudo systemctl restart mongod
</pre>
<pre> 
sudo lsof -i | grep mongo
</pre>


