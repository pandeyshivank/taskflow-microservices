import { useActionState, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
interface FormState {
  message?: string;
  success?: boolean;
  title?: string;
  description?: string;
  dueDate?: string;
  status?: string;
  id?: string;
}

const initialFormState: FormState = {
  success: false,
  message: "",
};

function UpdateTask() {
  const { taskId } = useParams();
  const [TaskData, setdata] = useState<FormState>();

  useEffect(() => {
    async function getTaskData() {
      const url = "https://localhost:7058/api/Task/GetTask/" + taskId;
      const UserResponsce = await fetch(url, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });
      const data: FormState = await UserResponsce.json();
      console.log(data);
      setdata(data);
      console.log(TaskData);
    }
    getTaskData();
  }, [taskId]);

  async function Handleform(
    prevState: FormState,
    formData: FormData,
  ): Promise<FormState> {
    const data: FormState = {
      title: formData.get("title") as string,
      description: formData.get("description") as string,
      dueDate: formData.get("dueDate") as string,
      status: formData.get("status") as string,
      id: taskId,
    };
    if (!data.title) {
      return {
        success: false,
        message: "Title is required!",
      };
    }
    const response = await fetch("https://localhost:7058/api/Task/UpdateTask", {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
      body: JSON.stringify(data),
    });
    if (!response.ok) {
      return {
        success: false,
        message: `Server returned error status: ${response.status}`,
      };
    }
    return {
      success: true,
      message: "Task Updated successfully!",
      ...data, // Spreads the created task properties into state if needed
    };
  }

  const [state, formAction, pending] = useActionState(
    Handleform,
    initialFormState,
  );

  return (
    <>
      <h1>Create Task</h1>
      <form
        action={formAction}
        style={{
          display: "flex",
          flexDirection: "column",
          gap: "0.5rem",
          maxWidth: "300px",
        }}
      >
        <label htmlFor="title">Title</label>
        <input
          type="text"
          name="title"
          id="title"
          value={TaskData?.title}
          onChange={(e) => setdata({ ...TaskData, title: e.target.value })}
        ></input>
        <br></br>
        <label htmlFor="description">Description</label>
        <input
          type="text"
          name="description"
          id="description"
          value={TaskData?.description}
          onChange={(e) =>
            setdata({ ...TaskData, description: e.target.value })
          }
        ></input>
        <br></br>
        <label htmlFor="dueDate">DueDate</label>
        <input
          type="date"
          name="dueDate"
          id="dueDate"
          value={TaskData?.dueDate.split("T")[0]}
          onChange={(e) =>
            setdata({
              ...TaskData,
              dueDate: e.target.value,
            })
          }
        ></input>
        <br></br>
        <label htmlFor="status">Status</label>
        <select
          name="status"
          id="status"
          value={TaskData?.status}
          onChange={(e) => ({ ...TaskData, status: e.target.value })}
        >
          <option value="New">New</option>
          <option value="Pending">Pending</option>
          <option value="Completed">Completed</option>
        </select>
        <br></br>
        <button type="submit" disabled={pending}>
          {pending ? "Creating..." : "Submit"}
        </button>
      </form>
      {state.message && (
        <p
          style={{ color: state.success ? "green" : "red", marginTop: "1rem" }}
        >
          {state.message}
        </p>
      )}
    </>
  );
}
export default UpdateTask;
