/*
	Excel Spreadsheet to Array or Associative Array or HTML TABLE
	or CSV to Array or HTML TABLE Converter
	
	Created By: Jeff Baker
	Created On: 8/22/2014
	Copyright (C) 2014 Jeff Baker
	Version 1.1b
	Updated On: 8/25/2014
	
	http://www.seabreezecomputers.com/excel2array
	
*/
var textbox='';
var finished='';
var original='';
var array_type=2; // Default to regular array
// make array of rows
var rows = [];

function convert_it(converter)
{
	/* Note: In Excel spreadsheets Rows are horizontal and labeled with numbers
		Columns are vertical labeled with letters
		Note: To put a quote in a CSV file you use two quotes as in: , "Black ""Bear"" Diner",
	*/	
	
	if (document.getElementById('double_quote').checked)
		var quote = '"';
	else
		var quote = "'";
		
	var separater = '\t'; // Separate data by tabs or commas
		
	textbox = document.getElementById('textbox').value;
  
	// If they converted once and finished text is in finished
	// and is the same as what is in the textbox, then change textbox back to original
	if (finished == textbox)
		textbox = original;
	else
		original = document.getElementById('textbox').value;
		
	/* Count how many commas or tabs are in string.  
		If there are more commas than we will assume the data is comma separated
	*/
	var commas = textbox.split(',').length-1;
	var tabs = textbox.split('\t').length-1;
	if (commas > tabs)
		//separater = ',';
		//separater = /(".*?"|[^",\s]+)(?=\s*,|\s*$)/;
		separater = /,\s*(?=(?:[^"]|"[^"]*")*$)/; // This is regex for matching commas not in quotes
	
	document.getElementById('textbox').innerHTML = "Commas:"+commas+" Tabs:"+tabs+"<br>";
		
	// make array of lines 
	var lines = textbox.split(/\r\n|\r|\n/g);
	
	// Reset array of rows
	rows = [];
	
	// Iterate through lines and put in 2D array 'rows'
	for (var i = 0; i < lines.length; i++)
	{
		rows[i] = lines[i].split(separater);
		
		
		for (var a= 0; a < rows[i].length; a++)
		{
			// Remove quotes around cell if first and last character are a quote
			if ((rows[i][a].charAt(0) == '"' && rows[i][a].charAt(rows[i][a].length-1) == '"')
				|| (rows[i][a].charAt(0) == "'" && rows[i][a].charAt(rows[i][a].length-1) == "'"))
			{
				rows[i][a] = rows[i][a].substr(1); // Remove first character
				rows[i][a] = rows[i][a].slice(0, -1); // Remove last character	
			}
			// Print it out for testing
			//document.getElementById('test').innerHTML += encodeURI(rows[i][a]) + ", ";
		}
		//document.getElementById('test').innerHTML += "<br>"; 
	}
	
	/* Clean up: If there is a blank line at bottom of textarea
		it is being put in the last row with as a blank array.
		We need to remove it
	*/
	if (rows[rows.length-1].length == 1 && rows[rows.length-1][0] == "")
		rows.pop(); 
	
	
	
	// test different browsers for CRLF \r\n
	//textbox = textbox.replace(/\r/g, "CR");
	//textbox = textbox.replace(/\n/g, "LF");
	/*	Chrome: LF
		IE: LF
		FF: LF
	*/
	if (converter == 1) // Convert to table
	{
		textbox = "<table border='1'>";
		for (var x = 0; x < rows.length; x++)
		{	
			textbox += "<tr>";
			for (var y = 0; y < rows[x].length; y++)
			{
				if (rows[x][y] || !document.getElementById('blank_fields').checked) // if field not empty
				{
					if ((x==0 && document.getElementById('row_headers').checked)
						|| y==0 && document.getElementById('col_headers').checked)
						textbox += "<th>" + rows[x][y] + "</th>";
					else	
						textbox += "<td>" + rows[x][y] + "</td>";
				}
			}
			textbox += "</tr>\n";	
		}
		textbox += "</table>";
		
		document.getElementById('test').innerHTML = "<b>Preview:</b><br>" + textbox;
	}
	else if (converter == 2) // Convert to array
	{
		array_type = 2;
		textbox = "";
		
		for (var x = 0; x < rows.length; x++)
		{	
			for (var y = 0; y < rows[x].length; y++)
			{
				if (rows[x][y] || !document.getElementById('blank_fields').checked) // if field not empty
				{	
					if (document.getElementById('numbers_to_string').checked || isNaN(rows[x][y]) || rows[x][y] == "") // if is Not a Number
						textbox += quote + rows[x][y] + quote;
					else
						textbox += rows[x][y];
					if (x < rows.length-1 || y < rows[x].length-1) textbox += ", ";
				}
			}
			if (document.getElementById('newlines').checked)
				textbox += "\n";
		
		}
	
	}
	else if (converter == 3) // Convert to asssociative array 
	{
		array_type = 3;
		if (document.getElementById('top').checked) // keys are in top row
		{
			if (document.getElementById('js').checked) // (javascript)
			{
				textbox = "[\n";
				
				for (var x = 1; x < rows.length; x++) // start at 1 because row 0 is the keys
				{	
					textbox += "{";
					for (var y = 0; y < rows[x].length; y++)
					{
						if (rows[x][y] || !document.getElementById('blank_fields').checked) // if field not empty
						{	
							if (y > 0) textbox += ", "; // Version 1.1b
							if (document.getElementById('newlines').checked)
								textbox += "\n\t";
							if (document.getElementById('numbers_to_string').checked || isNaN(rows[x][y]) || rows[x][y] == "") // if is Not a Number
								textbox += quote + rows[0][y] + quote + ": " +
										quote + rows[x][y] + quote;
							else
								textbox += quote + rows[0][y] + quote + ": " +
										 rows[x][y];
							//if (y < rows[x].length-1) textbox += ", "; // Version 1.1b - Removed
						}
					}
					textbox += "}";
					if (x < rows.length-1) textbox += ", ";
					if (document.getElementById('newlines').checked)
						textbox += "\n";
				
				}	
				textbox += "]";
			}
			else // PHP
			{
				textbox = "$arr = array(\n";
				
				for (var x = 1; x < rows.length; x++) // start at 1 because row 0 is the keys
				{	
					textbox += "\tarray(";
					for (var y = 0; y < rows[x].length; y++)
					{
						if (rows[x][y] || !document.getElementById('blank_fields').checked) // if field not empty
						{
							if (y > 0) textbox += ", "; // Version 1.1b
							if (document.getElementById('newlines').checked)
								textbox += "\n\t\t";
							if (document.getElementById('numbers_to_string').checked || isNaN(rows[x][y]) || rows[x][y] == "") // if is Not a Number
								textbox +=  quote + rows[0][y] + quote + " => " +
											quote + rows[x][y] + quote;
							else
								textbox += quote + rows[0][y] + quote + " => " +
											rows[x][y];		
							//if (y < rows[x].length-1) textbox += ", "; // Version 1.1b - Removed
						}
					}
					textbox += ")";
					if (x < rows.length-1) textbox += ", ";
					if (document.getElementById('newlines').checked)
						textbox += "\n";
				
				}	
				textbox += ");";
			}
		}
		else // keys are in left column
		{
			if (document.getElementById('js').checked) // (javascript)
			{
				textbox = "[\n";
				var more=1; // Let's us know if there is more columns (y)
				for (var y=1; more > 0; y++) // start at 1 because column 0 is the keys
				{
					more = 0;  // Will change to 1 below if we need to go through loop again
					textbox += "{";
					for (var x = 0; x < rows.length; x++)
					{
						if (y < rows[x].length) // if there is data in this cell
						{
							if (rows[x][y] || !document.getElementById('blank_fields').checked) // if field not empty
							{
								if (x > 0) textbox += ", "; // Version 1.1b
								if (x > 0 && document.getElementById('newlines').checked)
									textbox += "\n";
								if (document.getElementById('numbers_to_string').checked || isNaN(rows[x][y]) || rows[x][y] == "") // if is Not a Number
									textbox += quote + rows[x][0] + quote + ": " +
											quote + rows[x][y] + quote;
								else
									textbox += quote + rows[x][0] + quote + ": " +
											 rows[x][y];
								//if (x < rows.length-1) textbox += ", "; // Version 1.1b - Removed
									
								// Check to see if there is more y
								if (y < rows[x].length-1)
									more = 1;
							}
						}
					}	
					textbox += "}";
					if (more == 1)
						textbox += ", "; 
					if (document.getElementById('newlines').checked)
						textbox += "\n";			
				}
					
				textbox += "]";
				
			}							
			else // PHP
			{					
				textbox = "$arr = array(\n";
				var more=1; // Let's us know if there is more columns (y)
				for (var y=1; more > 0; y++) // start at 1 because column 0 is the keys
				{
					more = 0;  // Will change to 1 below if we need to go through loop again
					textbox += "\tarray(";
					for (var x = 0; x < rows.length; x++)
					{
						if (y < rows[x].length) // if there is data in this cell
						{
							if (rows[x][y] || !document.getElementById('blank_fields').checked) // if field not empty
							{
								if (x > 0) textbox += ", "; // Version 1.1b
								if (document.getElementById('newlines').checked)
									textbox += "\n\t\t"	
								if (document.getElementById('numbers_to_string').checked || isNaN(rows[x][y]) || rows[x][y] == "") // if is Not a Number
									textbox += quote + rows[x][0] + quote + " => " +
											quote + rows[x][y] + quote;
								else
									textbox += quote + rows[x][0] + quote + ": " +
											 rows[x][y];
								//if (x < rows.length-1) textbox += ", "; // Version 1.1b - Removed
								
								//if (document.getElementById('newlines').checked)
								//	textbox += "\n";	
								// Check to see if there is more y
								if (y < rows[x].length-1)
									more = 1;
							}
						}
					}	
					textbox += ")";
					if (more == 1)
						textbox += ", "; 
					if (document.getElementById('newlines').checked)
						textbox += "\n";			
				}
					
				textbox += ");";
			}
		}
		
	}
	
	 
	
	document.getElementById('textbox').value = textbox; // convert the textarea
	finished = textbox; // make finished the same as textbox


} // end function convert_it(converter)

function back_to_original()
{
	document.getElementById('textbox').value = original;
}

