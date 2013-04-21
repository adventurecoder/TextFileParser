TextFileParser
==============

A simple project to read a large text file and count the number of occurences of unique words utilising .Net 4.5 TPL. Potentially, it can be improved to read multiple files and do more processing than just counting words. 

----------------
-- ASSUMPTION --
----------------

* .Net 4.5 is installed in the test machine. 

* Implementation is done using the buffered way despite the unbuffered approach is proven to be faster according to Microsoft Research paper: http://research.microsoft.com/apps/pubs/default.aspx?id=64538 
Decision is made with assumption that 8 minutes and 2 seconds processing time for 3,137,916 Kilobytes of file is acceptable, and that the buffered code using .Net library is less complex and better for maintainability. The 8 minutes and 2 seconds is the result of running in: Windows 8 Pro running natively in a partitioned MBP with x64-based Intel Core i7-2640M CPU @ 2.80GHz, 2 Cores, 4 Logical Processors and 4GB RAM. 

* The bottleneck is identified to be the reader. The number of parallel threads for wordCounters is 128, testing with more threads didn't show any benefit in speed. 

* During testing, the BlockingCollection<string> list threw out of memory exception when the list reached around 45357892 entries. Therefore, it is coded that the FileReader will sleep for a second when the list is over 45357000 entries. This is to wait for the wordCounters to take items off the list. Given more time, it will be better without magic number and set the number based on the used memory. 

* I decided not to use shims to test FileStream, StreamReader and StreamWriter because it may not give much benefit (only increasing test code coverage). Shims was not recommended as mentioned in Microsoft ALM 2012 event I attended, only meant to use it cautiously. 

* UI is not the main focus so I created a runnable console instead of a WPF application. Feel free to add more to this :)

----------
-- IDEA --
----------

* May be breaking down more of the tasks into smaller parallel-friendly chunks can make the speed faster. 

* It would be good to take out the magic string and magic number. 

* Instead of reading just one file, this project can be extended to read multiple files.

* Instead of just counting words, it can be used to grab more important information from the file. 

* Strip off HTML tag? I tried using Regex but the performance suffers terribly...

---------------------
-- TO RUN THE TEST --
---------------------

1. Ensure that TextFileParserConsole is the start up project. 
2. Run the project (F5).
3. Insert the full file path of the test document and enter.
4. Insert the full file path for the result document to be saved to. 
5. The TextFileParser will display the result and duration upon completion. 
