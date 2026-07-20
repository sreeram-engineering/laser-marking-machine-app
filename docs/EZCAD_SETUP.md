# EZCAD2 Text File Setup

Use EZCAD2 v2.14.10 only as the marking engine.

1. Create or open the part template in EZCAD.
2. Add the QR/data-matrix object.
3. Configure the QR object to read data from a text file.
4. Point it to:

   ```text
   C:\Laser\QRDATA.TXT
   ```

5. Save the `.ezd` template.
6. In the app, set the part template path to that `.ezd` file and press `Set Active`.

The app writes one QR payload to the same `QRDATA.TXT` file per serial number. It updates the file in place so EZCAD's text-file QR object stays connected between marks. Operators should not edit, delete, replace, or reselect the QR file inside EZCAD during production.
