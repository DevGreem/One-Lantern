
public enum RegisterType
{
	ADD, // Add a new record, if already exists don't add the new record
	EDIT, // Edit an existent record, if don't exists, create a new record
	REPLACE // Replace an existent record, if don't exists, create a new record
}