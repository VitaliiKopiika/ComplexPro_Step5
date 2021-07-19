Run a DOS application and capture its output in C#
Posted on January 29, 2015 by Rod Stephens
[DOS]


Enter the name of a DOS program or batch file in the text box and click the Run button to execute the following code.

// Run the DOS program.
private void btnRun_Click(object sender, EventArgs e)
{
    // Set start information.
    ProcessStartInfo start_info =
        new ProcessStartInfo(txtProgram.Text);
    start_info.UseShellExecute = false;
    start_info.CreateNoWindow = true;
    start_info.RedirectStandardOutput = true;
    start_info.RedirectStandardError = true;

    // Make the process and set its start information.
    using (Process proc = new Process())
    {
        proc.StartInfo = start_info;

        // Start the process.
        proc.Start();

        // Attach to stdout and stderr.
        using (StreamReader std_out = proc.StandardOutput)
        {
            using (StreamReader std_err = proc.StandardError)
            {
                // Display the results.
                txtStdout.Text = std_out.ReadToEnd();
                txtStderr.Text = std_err.ReadToEnd();

                // Clean up.
                std_err.Close();
                std_out.Close();
                proc.Close();
            }
        }
    }
}
This code creates a ProcessStartInfo object to hold information about the process that it should start. It then creates a Process object, sets its StartInfo property, and calls its Start method to start it.

Next the program creates StreamReader objects to get the process’s standard output and standard error streams. It reads the streams and displays their contents.

Finally the program closes the streams and the Process.

The sample batch file included with this example echoes a couple of messages and then tries to execute an invalid command so it produces output in stdout and stderr.

