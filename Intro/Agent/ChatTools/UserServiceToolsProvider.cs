using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using System.ClientModel;
using OpenAI.Chat;
using System.Text.Json;
using Agent.Services;

namespace Agent.ChatTools
{
    public class UserServiceToolsProvider : IChatToolProvider
    {
        private readonly UserService _userService;

        public UserServiceToolsProvider(UserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public List<ChatTool> GetChatTools()
        {
            return new List<ChatTool>
                        {
                            ChatTool.CreateFunctionTool(
                                functionName: "get_user_by_id",
                                functionDescription: "Get a user by their ID",
                                functionParameters: BinaryData.FromString("""
                                {
                                    "type": "object",
                                    "properties": {
                                        "id": {
                                            "type": "integer",
                                            "description": "The user ID"
                                        }
                                    },
                                    "required": ["id"]
                                }
                                """)
                            ),
                            ChatTool.CreateFunctionTool(
                                functionName: "get_all_users",
                                functionDescription: "Get all users in the system",
                                functionParameters: BinaryData.FromString("""
                                {
                                    "type": "object",
                                    "properties": {}
                                }
                                """)
                            ),
                            ChatTool.CreateFunctionTool(
                                functionName: "create_user",
                                functionDescription: "Create a new user with all details",
                                functionParameters: BinaryData.FromString("""
                                {
                                    "type": "object",
                                    "properties": {
                                        "id": {
                                            "type": "integer",
                                            "description": "The user ID"
                                        },
                                        "username": {
                                            "type": "string",
                                            "description": "The username for the new user"
                                        },
                                        "email": {
                                            "type": "string",
                                            "description": "The email for the new user"
                                        },
                                        "city": {
                                            "type": "string",
                                            "description": "The city where the user is located"
                                        },
                                        "jobTitle": {
                                            "type": "string",
                                            "description": "The job title of the user"
                                        }
                                    },
                                    "required": ["id", "username", "email", "city", "jobTitle"]
                                }
                                """)
                            ),
                            ChatTool.CreateFunctionTool(
                                functionName: "update_user",
                                functionDescription: "Update an existing user with all details",
                                functionParameters: BinaryData.FromString("""
                                {
                                    "type": "object",
                                    "properties": {
                                        "id": {
                                            "type": "integer",
                                            "description": "The user ID to update"
                                        },
                                        "username": {
                                            "type": "string",
                                            "description": "The new username"
                                        },
                                        "email": {
                                            "type": "string",
                                            "description": "The new email"
                                        },
                                        "city": {
                                            "type": "string",
                                            "description": "The new city"
                                        },
                                        "jobTitle": {
                                            "type": "string",
                                            "description": "The new job title"
                                        }
                                    },
                                    "required": ["id", "username", "email", "city", "jobTitle"]
                                }
                                """)
                            ),
                            ChatTool.CreateFunctionTool(
                                functionName: "delete_user",
                                functionDescription: "Delete a user by ID",
                                functionParameters: BinaryData.FromString("""
                                {
                                    "type": "object",
                                    "properties": {
                                        "id": {
                                            "type": "integer",
                                            "description": "The user ID to delete"
                                        }
                                    },
                                    "required": ["id"]
                                }
                                """)
                            )
                        };
        }

        public string CallChatTool(ChatToolCall toolCall)
        {
            var functionName = toolCall.FunctionName;
            var arguments = toolCall.FunctionArguments.ToString();

            return functionName switch
            {
                "get_user_by_id" => HandleGetUserById(arguments),
                "get_all_users" => HandleGetAllUsers(),
                "create_user" => HandleCreateUser(arguments),
                "update_user" => HandleUpdateUser(arguments),
                "delete_user" => HandleDeleteUser(arguments),
                _ => $"Unknown function: {functionName}"
            };
        }

        private string HandleGetUserById(string arguments)
        {
            try
            {
                var args = JsonSerializer.Deserialize<JsonElement>(arguments);
                var id = args.GetProperty("id").GetInt32();

                var user = _userService.GetUserById(id);
                if (user == null)
                    return $"User with ID {id} not found.";

                return JsonSerializer.Serialize(user);
            }
            catch (Exception ex)
            {
                return $"Error getting user: {ex.Message}";
            }
        }

        private string HandleGetAllUsers()
        {
            try
            {
                var users = _userService.GetAllUsers();
                return JsonSerializer.Serialize(users);
            }
            catch (Exception ex)
            {
                return $"Error getting all users: {ex.Message}";
            }
        }

        private string HandleCreateUser(string arguments)
        {
            try
            {
                var args = JsonSerializer.Deserialize<JsonElement>(arguments);
                var user = new User
                {
                    Id = args.GetProperty("id").GetInt32(),
                    UserName = args.GetProperty("username").GetString() ?? string.Empty,
                    Email = args.GetProperty("email").GetString() ?? string.Empty,
                    City = args.GetProperty("city").GetString() ?? string.Empty,
                    JobTitle = args.GetProperty("jobTitle").GetString() ?? string.Empty
                };

                var createdUser = _userService.CreateUser(user);
                return JsonSerializer.Serialize(createdUser);
            }
            catch (Exception ex)
            {
                return $"Error creating user: {ex.Message}";
            }
        }

        private string HandleUpdateUser(string arguments)
        {
            try
            {
                var args = JsonSerializer.Deserialize<JsonElement>(arguments);
                var user = new User
                {
                    Id = args.GetProperty("id").GetInt32(),
                    UserName = args.GetProperty("username").GetString() ?? string.Empty,
                    Email = args.GetProperty("email").GetString() ?? string.Empty,
                    City = args.GetProperty("city").GetString() ?? string.Empty,
                    JobTitle = args.GetProperty("jobTitle").GetString() ?? string.Empty
                };

                var success = _userService.UpdateUser(user);
                return success ? "User updated successfully." : $"User with ID {user.Id} not found.";
            }
            catch (Exception ex)
            {
                return $"Error updating user: {ex.Message}";
            }
        }

        private string HandleDeleteUser(string arguments)
        {
            try
            {
                var args = JsonSerializer.Deserialize<JsonElement>(arguments);
                var id = args.GetProperty("id").GetInt32();

                var success = _userService.DeleteUser(id);
                return success ? "User deleted successfully." : $"User with ID {id} not found.";
            }
            catch (Exception ex)
            {
                return $"Error deleting user: {ex.Message}";
            }
        }
    }
}
