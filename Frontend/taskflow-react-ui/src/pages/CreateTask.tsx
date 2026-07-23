import { useActionState } from "react";

interface FormState {
  message?: string;
  success?: boolean;
  title?: string;
  description?: string;
  dueDate?: string;
  status?: string;
}

const initialFormState: FormState = {
  success: false,
  message: "",
};

function CreateTask() {
  async function Handleform(
    prevState: FormState,
    formData: FormData,
  ): Promise<FormState> {
    const data: FormState = {
      title: formData.get("title") as string,
      description: formData.get("description") as string,
      dueDate: formData.get("dueDate") as string,
      status: formData.get("status") as string,
    };
    if (!data.title) {
      return {
        success: false,
        message: "Title is required!",
      };
    }
    const response = await fetch("https://localhost:7058/api/Task/CreateTask", {
      method: "POST",
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
      message: "Task created successfully!",
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
        <input type="text" name="title" id="title"></input>
        <br></br>
        <label htmlFor="description">Description</label>
        <input type="text" name="description" id="description"></input>
        <br></br>
        <label htmlFor="dueDate">DueDate</label>
        <input type="date" name="dueDate" id="dueDate"></input>
        <br></br>
        <label htmlFor="status">Status</label>
        <select name="status" id="status">
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
export default CreateTask;
