export const QuestionsForm = () => {
    return (
        <div className="w-full">
            <label className="block text-2xl font-medium mb-2" htmlFor="pregunta">Pregunta</label>
            <section className="w-full">
                 <textarea id="pregunta" className="w-full h-60 p-3 border rounded resize-none mb-4"></textarea>
            </section>
            <button className="bg-indigo-900 hover:bg-indigo-800 text-white font-semibold px-6 py-2 rounded">Crear</button>
        </div>
    )

}