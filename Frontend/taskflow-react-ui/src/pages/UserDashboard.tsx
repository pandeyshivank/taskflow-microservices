import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

interface TaskResponse {
  id: string;
  title: string;
  description: string;
  dueDate: string;
  status: string;
  userId: string;
}

function UserDashBoard() {
  const [taskList, setTaskList] = useState<TaskResponse[]>([]);
  const [deletingId, setDeletingId] = useState<string | null>(null);

  async function handleDelete(id: string) {
    try {
      const isconfirmed = window.confirm(
        "Are you sure want to delete this task?",
      );

      if (!isconfirmed) {
        return;
      }
      setDeletingId(id);
      const deleteResponse = await fetch(
        "https://localhost:7058/api/Task/DeleteTask/" + id,
        {
          method: "DELETE",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        },
      );
      if (!deleteResponse.ok) {
        throw new Error(`HTTP error! Status: ${deleteResponse.status}`);
      }
      const result = await deleteResponse.json();
      setTaskList((prevTasks) => prevTasks.filter((task) => task.id !== id));
      return true;
    } catch {
    } finally {
      setDeletingId("");
    }
  }

  useEffect(() => {
    async function loadTask() {
      var response = await fetch(
        "https://localhost:7058/api/Task/GetAllUserTask",
        {
          method: "GET",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        },
      );
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
      const data = await response.json();
      console.log(data);
      setTaskList(data);
    }
    loadTask();
  }, []);

  return (
    <>
      <h1>Assign Task Dashboard</h1>
      <Link to="/createTask">Create New Task</Link>
      <table>
        <thead>
          <tr>
            <td>Task Id</td>
            <td>Title</td>
            <td>Status</td>
            <td>Description</td>
            <td>DueDate</td>
            <td>Action</td>
          </tr>
        </thead>
        <tbody>
          {taskList.map((x) => (
            <tr key={x.id}>
              <td>{x.id}</td>
              <td>{x.title}</td>
              <td>{x.status}</td>
              <td>{x.description}</td>
              <td>{x.dueDate}</td>
              <td>
                <Link to={"/updateTask/" + x.id}>Edit</Link>
              </td>
              <td>
                <button onClick={() => handleDelete(x.id)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </>
  );
}
export default UserDashBoard;
